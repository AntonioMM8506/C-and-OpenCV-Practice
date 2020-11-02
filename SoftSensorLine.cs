//Antonio Manilla Maldonado
//SMV
//Final Exam

using System;
using OpenCvSharp;

namespace Exam_002
{
    public class SoftSensorLine : SoftSensor
    {
        //Private numbers
        private double[] Position = { 10, 10, 10, 10 };
        private bool evalResult = false;
        private double meanEval = 0;
        private double maxEval = 0;
        private double minEval = 0;
        private double evalDataValAvg = 0;
        private double evalDataValMax = 0;
        private double evalDataValMin = 0;
        private double size = 0;
        private double thres = 0;
        ThresHoldTypeEnum type = ThresHoldTypeEnum.Dark;

        public SoftSensorLine(ThresHoldTypeEnum thresType, double[] position, double size, double threshold)
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

            //-----------------------------------------------------------------------------------------------------------------
            double x = Math.Abs(Position[0] - Position[2]);
            double y = Math.Abs(Position[1] - Position[3]);
            double mid = Math.Sqrt((x*x)+(y*y));
            double xc = x + mid;
            double yc = y - mid;
            Point center;
            center.X = (int)xc;
            center.Y = (int)yc;
            double angle = Math.Acos(x/mid) * 180 / Math.PI;

            if (angle<45)
            {
                angle += 90;
            }

            Mat mask3 = Mat.Ones(image.Size(), MatType.CV_8UC1);
            mask3.Line(new Point(Position[0], Position[1]), new Point(Position[2], Position[3]), Scalar.Black, 2);

            meanEval = Cv2.Mean(image, mask: mask3).Val1;
            evalDataValAvg = meanEval;

            Console.WriteLine(evalDataValAvg);

            //Max
            if (maxEval < meanEval)
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
            if (meanEval < 100 && meanEval >= 0)
            {
                eval = true;
            }
            else if (meanEval < 255 && meanEval > 100)
            {
                eval = false;
            }

            evalResult = eval;

            return evalResult; 

        }//End of Evaluate


        public override void DrawResult(Mat image)
        {
            //Draw inspection result in image, make indication green if OK and red if NOK
            //Fill in code for evaluation according to specification
            //Tip: Only do something if params are not default

            var OK = new Scalar(0, 255, 0);
            var Fail = new Scalar(0, 0, 255);

            Cv2.Line(image, new Point(50,50), new Point(200, 200),OK, 4 );
            //Cv2.Line(image, new Point(Position[0], Position[1]), new Point(Position[2], Position[3]), evalResult?OK:Fail, 4);

        }//End of DrawResult


        public override bool SetPosition(double[] position)
        {
            //Set coord  inates for the softsensor, required length of the array will depend on SoftSensor type, Control Point[2] or Control Line[4]
            //Returns true if operation, ie correct length of params array, otherwise false.
            //If the array length is not correct the point should be set to default values that are not 0.

            if (position.Length != 4)
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
                size = 2;
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
                type = ThresHoldTypeEnum.Dark;
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
                return SoftSensorTypeEnum.Line;
            }

        }// = SoftSensorTypeEnum.Point;



    }//End of class Line

}//End of namespace
