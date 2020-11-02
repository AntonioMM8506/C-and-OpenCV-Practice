//Antonio Manilla Maldonado
//SMV
//Final Exam

using System;
using OpenCvSharp;

namespace Exam_002
{
    public class SoftSensorPoint : SoftSensor
    {
        //Private numbers
        private double[] Position = { 10, 10 };
        private bool evalResult = false;
        private double meanEval = 0;
        private double maxEval = 0;
        private double minEval = 0;
        private double evalDataValAvg = 0;
        private double evalDataValMax = 0;
        private double evalDataValMin = 0;
        private double size = 0;
        private double thres = 0;
        ThresHoldTypeEnum type = ThresHoldTypeEnum.Bright;

        public SoftSensorPoint(ThresHoldTypeEnum thresType, double[] position, double size, double threshold)
        {
            //Parametized constructor
            //Start with setting position to check if correct number of arguments, if not return default.

            if (SetPosition(position))
            {
                Size = size;
                ThresType = thresType;
                Threshold = threshold;
            }
           

        }//End of SoftSensorPoint


        public override bool Evaluate(Mat image)
        {
            //Evaluating the softSensor on image, image returning the outcome true for ok and false if the criterions are not fulfilled.
            //Fill in code for Evaluation according to specification
            //Tip: only do something if params are not default
            bool eval = false;
            //double maxVal = 0;
            //meanEval = 0; 

            //Creates the mask of zeros and a circle inside it, after that. with the method Mean, and using the mentioned mask.
            //One can calculate de Average value in a circular ROI
            Mat mask2 = Mat.Zeros(image.Size(), MatType.CV_8UC1);
            mask2.Circle(new Point((int)Position[0], (int)Position[1]), (int)Math.Round(Size), Scalar.White, -1);

            meanEval = Cv2.Mean(image, mask:mask2).Val0;
            evalDataValAvg = meanEval;

            //Max
            if (maxEval<meanEval)
            {
                maxEval = meanEval;
            }
            evalDataValMax = maxEval;

            //Min
            if (minEval > meanEval)
            {
                minEval = meanEval;
            }
            evalDataValMin = minEval;

            //Mean - Average
            if (meanEval<255 && meanEval>=100)
            {
                eval = true;
            }
            else if(meanEval<100 && meanEval>0)
            {
                eval = false;
            }

            evalResult = eval;

            return evalResult; //CHANGE!!! to return result of inspection

        }//End of Evaluate


        public override void DrawResult(Mat image)
        {
            //Draw inspection result in image, make indication green if OK and red if NOK
            //Fill in code for evaluation according to specification
            //Tip: Only do something if params are not default

            var OK = new Scalar(0, 255, 0);
            var Fail = new Scalar(0, 0, 255);

            Cv2.Circle(image, new Point(Position[0], Position[1]), (int)Math.Round(Size), evalResult?OK:Fail,-1);
            
        }//End of DrawResult


        public override bool SetPosition(double[] position)
        {
            //Set coord  inates for the softsensor, required length of the array will depend on SoftSensor type, Control Point[2] or Control Line[4]
            //Returns true if operation, ie correct length of params array, otherwise false.
            //If the array length is not correct the point should be set to default values that are not 0.

            if (position.Length != 2)
                return false;

            Position = position;
            return true;
        }




        //Size of softsensor, radius for control point and line thickness for control line.
        public override double Size
        {
            get
            {
                return size;
            }
            set
            {
                size = 3;
            }

        }//End of Size
        
        
        //Threshold type of the soft sensor.
        public override ThresHoldTypeEnum ThresType
        {
            get
            {
                return type;
            }
            set
            {
                type = ThresHoldTypeEnum.Bright;
            }

        }//End of ThresHoldTypeEnum
        

          
        //Threshold value.
        public override double Threshold
        {
            get
            {
                return thres;
            }
            set
            {
                thres = 100;    
            }

        }//End of Threshold
        

           
        //Read only and only valid fater Evaluate, returns the status of the Control Point, true if OK and flase if NOK.
        public override bool EvalResult
        {
            get
            {
                return evalResult;
            }

        }//End of EvalResult
        

        
        //Read only and only valid after Evaluate, returns the appropiate average value used for evaluation of threshold.
        public override double EvalDataValAvg
        {
            get
            {
                evalDataValAvg = meanEval;
                return evalDataValAvg;
            }

        }//End of EvalDataValAvg
        

        
        //Read only and only valid after Evaluate, Returns the appropiate max value used for the evaluation of the threshold.
        public override double EvalDataValMax
        {
            get
            {
               
                return evalDataValMax;
            }

        }//End of EvalDataValMax
        


        //Read only and only valid after Evaluate, Returns the appropiate min value used for the evaluation of the threshold.
        public override double EvalDataValMin
        {
            get
            {
                
                return evalDataValMin;
            }

        }//End of EvalDataValMin

         
        //Read Only set in constructor, Sensor Type.
        public override SoftSensorTypeEnum SoftSensorType
        {
            get
            {
                return SoftSensorTypeEnum.Point;
            }

        }// = SoftSensorTypeEnum.Point;
        
    

    }//End of class Point


}//End of namespace
