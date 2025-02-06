using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProblemRedirection : MonoBehaviour
{
    public static string Text;
    
    private List<float> Numbers;

    public bool GetProblemPage()
    {
        Numbers = new List<float>();
        ExtractNumbers();
        Debug.Log(Text);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //GCF
        if (Text.ToLower().Contains(("GCF").ToLower()) || Text.ToLower().Contains(("G.C.F").ToLower())|| Text.ToLower().Contains(("greatest common factor").ToLower()))
        {
            try
            {
                GCFScript.FirstNum =((int)Numbers[0]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                GCFScript.SecNum = ((int)Numbers[1]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            PlayerPrefs.SetString("type", "GCF");
            SceneManager.LoadScene("GCF");

            return true;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //LCM
        else if (Text.ToLower().Contains(("LCM").ToLower()) || Text.ToLower().Contains(("L.C.M").ToLower()) || Text.ToLower().Contains(("greatest common factor").ToLower()))
        {
            try
            {
                GCFScript.FirstNum = ((int)Numbers[0]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                GCFScript.SecNum = ((int)Numbers[1]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            PlayerPrefs.SetString("type", "LCM");
            SceneManager.LoadScene("GCF");
            return true;

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //prime factors
        else if ((Text.ToLower().Contains(("PF"))) || (Text.ToLower().Contains(("prime factors"))) || (Text.ToLower().Contains(("prime"))) || (Text.ToLower().Contains(("factors"))) || (Text.ToLower().Contains(("P.F"))))
        {

            try
            {
                PrimeFactors.StaticNumber = ((int)Numbers[0]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            SceneManager.LoadScene("PrimeFactorsScene");

            return true;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //simplify
        else if ((Text.ToLower().Contains(("simplify"))) || (Text.ToLower().Contains(("simple"))) || (Text.ToLower().Contains(("simplest"))))
        {
            try
            {
                SimplifyScript.FirstNum = ((int)Numbers[0]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                SimplifyScript.SecNum = ((int)Numbers[1]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            SceneManager.LoadScene("SimplifyScene");
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //solving 2eqs
        else if (((Text.ToLower().Contains(("two equations"))) || (Text.ToLower().Contains(("two following equations")))) && ((!Text.ToLower().Contains(("//"))  && (!Text.ToLower().Contains(("parallel")) && (!Text.ToLower().Contains(("perpendiculat")))))))
        {
            try
            {
                SolvingTwoEqs.FirstNum = ((int)Numbers[0]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }


            try
            {
                SolvingTwoEqs.SecNum = ((int)Numbers[1]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }


            try
            {
                SolvingTwoEqs.ThirdNum = ((int)Numbers[2]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                SolvingTwoEqs.ForthNum= ((int)Numbers[3]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                SolvingTwoEqs.FifthNum = ((int)Numbers[4]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                SolvingTwoEqs.SicthNum = ((int)Numbers[5]).ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            SceneManager.LoadScene("SolvingTwoEqs");
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //adding two fractinos
        else if (Text.ToLower().Contains(("fractions").ToLower()) || Text.ToLower().Contains(("fraction").ToLower()) || Text.ToLower().Contains(("adding two fractions").ToLower()))
        {
            int FNue =0, FDeno = 0, SNue = 0, SDeno = 0;

            try
            {
                FNue = int.Parse(((int)Numbers[0]).ToString());
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                FDeno = int.Parse(((int)Numbers[1]).ToString());
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            try
            {
                SNue = int.Parse(((int)Numbers[2]).ToString());
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
                   
            
            try
            {
                SDeno = int.Parse(((int)Numbers[3]).ToString());
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            AddingTwoFractionsScript.FirstNumber = FNue.ToString();
            AddingTwoFractionsScript.SecNumber = FDeno.ToString();
            AddingTwoFractionsScript.ThirdNumber = SNue.ToString();
            AddingTwoFractionsScript.FourthNumber = SDeno.ToString();

            SceneManager.LoadScene("AddingTwoFractionsScene");
            return true;
        }

        //sl eqs
        else if ((Text.ToLower().Contains(("straight").ToLower()) || Text.ToLower().Contains(("line").ToLower())) && !Text.ToLower().Contains(("prove").ToLower()) && !Text.ToLower().Contains(("value").ToLower()))           
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 9).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlEqsScene");

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //first sl problem
            if (Text.ToLower().Contains(("slope").ToLower()) && (!Text.ToLower().Contains(("intersection").ToLower()) && !Text.ToLower().Contains(("parallel").ToLower()) &&!Text.ToLower().Contains(("perpendicular").ToLower() )))
            {
                RedurectionUpdate.ProblemBools[0] = true;

                try
                {
                    FirstProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }      
                
                try
                {
                    FirstProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }   
                
                try
                {
                    FirstProblem.FNue = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                
                try
                {
                    FirstProblem.FDeno = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //sixth sl problem
            else if (Text.ToLower().Contains(("point").ToLower()) && Text.ToLower().Contains(("parallel").ToLower()) && Text.ToLower().Contains(("two points").ToLower()))
            {
                RedurectionUpdate.ProblemBools[5] = true;

                try
                {
                    SixthProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SixthProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SixthProblem.A = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SixthProblem.B = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                try
                {
                    SixthProblem.C = ((int)Numbers[4]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }               
                try
                {
                    SixthProblem.D = ((int)Numbers[5]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //seventh sl problem
            else if (Text.ToLower().Contains(("point").ToLower()) && Text.ToLower().Contains(("perpendicular").ToLower()) && Text.ToLower().Contains(("line").ToLower()))
            {
                RedurectionUpdate.ProblemBools[6] = true;

                try
                {
                    SeventhsProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SeventhsProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SeventhsProblem.A = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SeventhsProblem.B = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                try
                {
                    SeventhsProblem.C = ((int)Numbers[4]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                try
                {
                    SeventhsProblem.D = ((int)Numbers[5]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //third sl problem
            else if (Text.ToLower().Contains(("two points").ToLower()) && Text.ToLower().Contains(("intercept").ToLower()))
            {
                RedurectionUpdate.ProblemBools[2] = true;
                try
                {
                    ThirdProblem.X = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    ThirdProblem.Y = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SecProbelm.X2val = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //second sl problem
            else if (Text.ToLower().Contains(("two points").ToLower()))
            {
                RedurectionUpdate.ProblemBools[1] = true;

                try
                {
                    SecProbelm.X1val = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SecProbelm.Y1val = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SecProbelm.X2val = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    SecProbelm.Y2val = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //fourth sl problem
            else if (Text.ToLower().Contains(("point").ToLower()) && Text.ToLower().Contains(("parallel").ToLower()) && Text.ToLower().Contains(("line").ToLower()))
            {

                RedurectionUpdate.ProblemBools[3] = true;

                try
                {
                    FourthProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FourthProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FourthProblem.A = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FourthProblem.B = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                try
                {
                    FourthProblem.C = ((int)Numbers[4]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //fifth sl problem
            else if (Text.ToLower().Contains(("point").ToLower()) && Text.ToLower().Contains(("perpendicular").ToLower()) && Text.ToLower().Contains(("line").ToLower()))
            {
                RedurectionUpdate.ProblemBools[4] = true;

                try
                {
                    FifthProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FifthProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FifthProblem.A = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    FifthProblem.B = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                try
                {
                    FifthProblem.C = ((int)Numbers[4]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //eighth sl problem
            else if (Text.ToLower().Contains(("slope").ToLower()) && !Text.ToLower().Contains(("parallel").ToLower()) && Text.ToLower().Contains(("perpendicular").ToLower()))
            {
                RedurectionUpdate.ProblemBools[7] = true;

                try
                {
                    EightsProblem.Xval = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    EightsProblem.Yval = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    EightsProblem.FNue = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    EightsProblem.FDeno = ((int)Numbers[3]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //nineth sl problem
            else if (Text.ToLower().Contains(("slope").ToLower()) && !Text.ToLower().Contains(("axis").ToLower()) && !Text.ToLower().Contains(("length").ToLower()) && Text.ToLower().Contains(("intersection").ToLower()))
            {
                RedurectionUpdate.ProblemBools[8] = true;

                try
                {
                    NinethProblem.FNue = ((int)Numbers[0]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    NinethProblem.FDeno = ((int)Numbers[1]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                try
                {
                    NinethProblem.Yval = ((int)Numbers[2]).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }


            }
            


            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope first eq
        else if (Text.ToLower().Contains(("prove").ToLower()) && Text.ToLower().Contains(("straight").ToLower()) && Text.ToLower().Contains(("line").ToLower()) && Text.ToLower().Contains(("angle").ToLower()) && Text.ToLower().Contains(("parallel").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[0] = true;
            try
            {
                SlopeFirstProblem.X1Val = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeFirstProblem.Y1Val = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeFirstProblem.X2Val = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeFirstProblem.Y2Val = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeFirstProblem.AngleVal = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            return true;
        }     
        
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope second eq
        else if (Text.ToLower().Contains(("prove").ToLower()) && Text.ToLower().Contains(("straight").ToLower()) && Text.ToLower().Contains(("line").ToLower()) && Text.ToLower().Contains(("angle").ToLower()) && Text.ToLower().Contains(("perpendicular").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[1] = true;
            try
            {
                SlopeSecondProblem.X1Val = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeSecondProblem.Y1Val = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeSecondProblem.X2Val = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeSecondProblem.Y2Val = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeSecondProblem.AngleVal = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            return true;
        }      
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope third eq
        else if (Text.ToLower().Contains(("prove").ToLower()) && Text.ToLower().Contains(("straight").ToLower()) && Text.ToLower().Contains(("line").ToLower()) && Text.ToLower().Contains(("y").ToLower()) && Text.ToLower().Contains(("x").ToLower()) && Text.ToLower().Contains(("parallel").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[2] = true;
            try
            {
                ThirdSlopeProblem.X1Val = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                ThirdSlopeProblem.Y1Val = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                ThirdSlopeProblem.X2Val = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                ThirdSlopeProblem.Y2Val = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                ThirdSlopeProblem.AVal = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                ThirdSlopeProblem.BVal = ((int)Numbers[5]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            
            try
            {
                ThirdSlopeProblem.CVal = ((int)Numbers[6]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope Fourth eq
        else if ( Text.ToLower().Contains(("straight").ToLower()) && Text.ToLower().Contains(("line").ToLower()) && Text.ToLower().Contains(("y").ToLower()) && Text.ToLower().Contains(("x").ToLower()) && Text.ToLower().Contains(("perpendicular").ToLower()) && Text.ToLower().Contains(("two points").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[3] = true;
            try
            {
                SlopeForthProblem.X1Val = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeForthProblem.Y1Val = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeForthProblem.X2Val = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeForthProblem.Y2Val = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeForthProblem.AVal = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeForthProblem.BVal = ((int)Numbers[5]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            try
            {
                SlopeForthProblem.CVal = ((int)Numbers[6]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope Fifth eq
        else if (Text.ToLower().Contains(("straight").ToLower()) && Text.ToLower().Contains(("line").ToLower()) &&  Text.ToLower().Contains(("perpendicular").ToLower()) && Text.ToLower().Contains(("two points").ToLower()) && (Text.ToLower().Contains(("parallel").ToLower()) || Text.ToLower().Contains(("//").ToLower())))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[4] = true;
            try
            {
                FifthSlopeProblem.X1Val = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                FifthSlopeProblem.Y1Val = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                FifthSlopeProblem.X2Val = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                FifthSlopeProblem.Y2Val = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                FifthSlopeProblem.AngleVal = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope Sixth eq
        else if (Text.ToLower().Contains(("two straight lines").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[5] = true;
            try
            {
                SixthSlopeProblem.A1 = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SixthSlopeProblem.B1 = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SixthSlopeProblem.C1 = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SixthSlopeProblem.A2 = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SixthSlopeProblem.B2 = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            try
            {
                SixthSlopeProblem.C2 = ((int)Numbers[5]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope eighth eq
        else if (Text.ToLower().Contains(("points").ToLower()) && Text.ToLower().Contains(("not").ToLower()) && Text.ToLower().Contains(("collinear").ToLower()) && !Text.ToLower().Contains(("value of").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[7] = true;
            try
            {
                SlopeEighthProblem.A1 = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeEighthProblem.B1 = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeEighthProblem.C1 = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeEighthProblem.A2 = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SlopeEighthProblem.B2 = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            try
            {
                SlopeEighthProblem.C2 = ((int)Numbers[5]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // slope seventh eq
        else if (Text.ToLower().Contains(("points").ToLower()) && Text.ToLower().Contains(("collinear").ToLower()) && !Text.ToLower().Contains(("value of").ToLower()))
        {
            RedurectionUpdate.ProblemBools = Enumerable.Repeat(false, 10).ToList();
            RedurectionUpdate.IsCalledFromOutSide = true;
            SceneManager.LoadScene("SlopeEqsScene");
            RedurectionUpdate.ProblemBools[6] = true;
            try
            {
                SeventhSlopeProblem.A1 = ((int)Numbers[0]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SeventhSlopeProblem.B1 = ((int)Numbers[1]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SeventhSlopeProblem.C1 = ((int)Numbers[2]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SeventhSlopeProblem.A2 = ((int)Numbers[3]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }
            try
            {
                SeventhSlopeProblem.B2 = ((int)Numbers[4]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            try
            {
                SeventhSlopeProblem.C2 = ((int)Numbers[5]).ToString();
            }
            catch (Exception e)
            {

                Debug.Log(e);
            }

            return true;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //division
        else if (Text.ToLower().Contains(("Divide").ToLower()) || Text.ToLower().Contains(("over").ToLower()) || Text.ToLower().Contains(("Devide").ToLower()) || Text.ToLower().Contains(("Div").ToLower()) || Text.ToLower().Contains(("/").ToLower()) || Text.ToLower().Contains(("÷").ToLower()))
        {

            bool IsFirstNumInt = false;
            bool IsSecNumInt = false;

            try
            {
                IsFirstNumInt = Mathf.Abs(Numbers[0]) == (int)Mathf.Abs(Numbers[0]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                IsSecNumInt = Mathf.Abs(Numbers[1]) == (int)Mathf.Abs(Numbers[1]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }


            if (IsSecNumInt && IsFirstNumInt)
            {
                Numbers[1] = int.Parse(Numbers[1].ToString()[0].ToString());
                if (!Text.Contains(("Long").ToLower()))
                {
                    DivisionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                    DivisionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                    SceneManager.LoadScene("Divsion");
                }
                else
                {
                    LongDivisionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                    LongDivisionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();

                    SubtractionScript.FirstNumber = LongDivisionScript.FirstNumber;
                    SubtractionScript.SecNumber = LongDivisionScript.SecNumber;
                    SceneManager.LoadScene("LongDivision");

                }
            }
            else
            {
                try
                {
                    Regex regex = new Regex(@"^10{0,4}$");

                    DivideByScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                    if (regex.IsMatch(Numbers[1].ToString()))
                    {
                        DivideByScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                    }
                    else
                    {
                        DivideByScript.SecNumber = "10";

                    }

                    SceneManager.LoadScene("DivideBy");
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }

                return true;

            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //multiplication
        else if (Text.ToLower().Contains(("time").ToLower()) || Text.ToLower().Contains(("multiply").ToLower()) || Text.ToLower().Contains(("multiplied").ToLower()) || Text.ToLower().Contains(("times").ToLower()) || Text.ToLower().Contains(("*").ToLower()) || Text.ToLower().Contains((" x ").ToLower()))
        {

            bool IsFirstNumInt = false;
            bool IsSecNumInt = false;

            try
            {
                IsFirstNumInt = Mathf.Abs(Numbers[0]) == (int)Mathf.Abs(Numbers[0]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                IsSecNumInt = Mathf.Abs(Numbers[1]) == (int)Mathf.Abs(Numbers[1]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            if (IsSecNumInt && IsFirstNumInt)
            {

                if (Numbers[1].ToString().Length >=2)
                {
                    Numbers[1] = float.Parse(Numbers[1].ToString()[0] +""+Numbers[1].ToString()[1]);

                }

                TwoDigitsMultiplicationScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                TwoDigitsMultiplicationScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();

                AdditionScript.FirstNumber = TwoDigitsMultiplicationScript.FirstNumber;
                AdditionScript.SecNumber = TwoDigitsMultiplicationScript.SecNumber;
                SceneManager.LoadScene("TwoDigitsMultiplicationScene");

                return true;
            }
            else
            {
                try
                {
                    Regex regex = new Regex(@"^10{0,4}$");

                    MutiplyByScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                    if (regex.IsMatch(Numbers[1].ToString()))
                    {
                        MutiplyByScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                    }
                    else
                    {
                        MutiplyByScript.SecNumber = "10";

                    }

                    SceneManager.LoadScene("MutiplyBy");
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }

                return true;

            }

        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //addition
        else if (Text.ToLower().Contains(("plus").ToLower()) || Text.ToLower().Contains(("+").ToLower())|| Text.ToLower().Contains(("addition").ToLower()) || Text.ToLower().Contains(("add").ToLower()))
        {

            bool IsFirstNumInt = false;
            bool IsSecNumInt = false;


            try
            {
                IsFirstNumInt = Mathf.Abs(Numbers[0]) == (int)Mathf.Abs(Numbers[0]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                IsSecNumInt = Mathf.Abs(Numbers[1]) == (int)Mathf.Abs(Numbers[1]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }


            if (IsSecNumInt && IsFirstNumInt)
            {

                AdditionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                AdditionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                SceneManager.LoadScene("AdditionScene");
            }
            else
            {
                DecimmalScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                DecimmalScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();


                AdditionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                AdditionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                AdditionScript.IsBasic = false;
                SceneManager.LoadScene("DecimalScene");
                PlayerPrefs.SetString("type", "add");
            }
            return true;

        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //subtraction
        else if (Text.ToLower().Contains(("minus").ToLower()) || Text.ToLower().Contains(("subtract").ToLower())|| Text.ToLower().Contains(("subtraction").ToLower()) || Text.ToLower().Contains(("sub").ToLower()) ||  Text.ToLower().Contains(("-").ToLower()))
        {

            bool IsFirstNumInt = false;
            bool IsSecNumInt = false;

            try
            {
                IsFirstNumInt = Mathf.Abs(Numbers[0]) == (int)Mathf.Abs(Numbers[0]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                IsSecNumInt = Mathf.Abs(Numbers[1]) == (int)Mathf.Abs(Numbers[1]);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }



            if (IsSecNumInt && IsFirstNumInt)
            {
                SubtractionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                SubtractionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();
                SceneManager.LoadScene("SubtractionScene");
            }
            else
            {
                DecimmalScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                DecimmalScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();

                SubtractionScript.FirstNumber = Mathf.Abs(Numbers[0]).ToString();
                SubtractionScript.SecNumber = Mathf.Abs(Numbers[1]).ToString();

                AdditionScript.IsBasic = false;
                SceneManager.LoadScene("DecimalScene");
                PlayerPrefs.SetString("type", "sub");
            }
            return true;

        }



        return false;

    }

    private void ExtractNumbers()
    {
        string currentNumber = "";
        foreach (char latter in Text)
        {
            if (float.TryParse(latter.ToString(), out float num) || (latter =='-') || latter =='.')
            {
                if (latter != '-' && latter != '.')
                {
                    currentNumber += num;
                }
                else
                {
                    if(latter == '-' && !currentNumber.Equals(""))
                    {
                        Numbers.Add(float.Parse(currentNumber));
                        currentNumber = "";

                    }
                    currentNumber += latter;
                    if (latter == '.' && currentNumber.Equals(latter.ToString()))
                    {
                        currentNumber = "";
                    }

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    try
                    {
                        Numbers.Add(float.Parse(currentNumber));

                    }
                    catch (System.Exception e)
                    {

                        Debug.Log(e);
                    }
                    currentNumber = "";
                }
            }
        }
    }
}
