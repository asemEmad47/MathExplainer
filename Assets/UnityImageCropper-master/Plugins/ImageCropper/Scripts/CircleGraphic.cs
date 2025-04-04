using UnityEngine;
using UnityEngine.UI;

namespace ImageCropperNamespace
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class SquareGraphic : MaskableGraphic
    {
        public enum Mode { FillInside = 0, FillOutside = 1, Edge = 2 };

#pragma warning disable 0649
        [Header("-> The Real Deal <- <- <-")]
        [SerializeField]
        private Sprite renderSprite;

        [SerializeField]
        private Mode mode;

        [SerializeField]
        private float edgeThickness = 1;
#pragma warning restore 0649

        private Vector2 uv;
        private Color32 color32;

        private float width, height;
        private float deltaWidth, deltaHeight;

        public override Texture mainTexture { get { return renderSprite != null ? renderSprite.texture : s_WhiteTexture; } }

        protected override void Awake()
        {
            base.Awake();

            if (renderSprite != null)
            {
                Vector4 packedUv = UnityEngine.Sprites.DataUtility.GetOuterUV(renderSprite);
                uv = new Vector2((packedUv.x + packedUv.z) * 0.5f, (packedUv.y + packedUv.w) * 0.5f);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Rect r = GetPixelAdjustedRect();

            color32 = color;
            width = r.width * 0.5f;
            height = r.height * 0.5f;

            vh.Clear();

            Vector2 pivot = rectTransform.pivot;
            deltaWidth = r.width * (0.5f - pivot.x);
            deltaHeight = r.height * (0.5f - pivot.y);

            if (mode == Mode.FillInside)
            {
                FillInside(vh);
            }
            else if (mode == Mode.FillOutside)
            {
                FillOutside(vh);
            }
            else
            {
                GenerateEdges(vh);
            }
        }

        private void FillInside(VertexHelper vh)
        {
            vh.AddVert(new Vector3(-width + deltaWidth, -height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(width + deltaWidth, -height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(width + deltaWidth, height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(-width + deltaWidth, height + deltaHeight, 0f), color32, uv);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        private void FillOutside(VertexHelper vh)
        {
            FillInside(vh);
        }

        private void GenerateEdges(VertexHelper vh)
        {
            float innerWidth = width - edgeThickness;
            float innerHeight = height - edgeThickness;

            vh.AddVert(new Vector3(-width + deltaWidth, -height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(width + deltaWidth, -height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(width + deltaWidth, height + deltaHeight, 0f), color32, uv);
            vh.AddVert(new Vector3(-width + deltaWidth, height + deltaHeight, 0f), color32, uv);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        public override void Cull(Rect clipRect, bool validRect)
        {
            canvasRenderer.cull = false;
        }
    }
}
