//Antonio Manilla Maldonado
//SMV
//Final Exam

using System;
using OpenCvSharp;

namespace Exam_002
{
    public enum ThresHoldTypeEnum { Bright, Dark }
    public enum SoftSensorTypeEnum { Point, Line }

    public abstract class SoftSensor
    {

        public static SoftSensor CreateSoftSensor(SoftSensorTypeEnum type, ThresHoldTypeEnum thresType, double[] position, double size, double threshold)
        {
            //instantiates a new object of type and returns it upcast to SoftSensor
            //Shall check parameters and if position array length not correct for the selected control type it should return a null object, so your progam has to handle this.
            //Implementd in base class SoftSenor

            switch (type)
            {
                case SoftSensorTypeEnum.Point:
                    return new SoftSensorPoint(thresType, position, size, threshold);
                    break;
                case SoftSensorTypeEnum.Line:
                    return new SoftSensorPoint(thresType, position, size, threshold);
                    break;
                default:
                    return null;
                    break;
            }

        }//End of SoftSensor


        //Evaluating the SoftSensor on image. Image returning the outcome true for ok and false if the criterions are not fulfilled.
        public abstract bool Evaluate(Mat image);

        //Draw Inspection result in image, make indication green if OK and red if NOK
        public abstract void DrawResult(Mat image);

        //Set coordinates for the SoftSensor, required length of the array will depend of the sof sensor type, Control Point[2] or Control Line [4].
        //Returns true if operation succesful, ie correct length of params array, otherwise false.
        //If the array length is not correct the point should be set to default values that are not 0.
        public abstract bool SetPosition(double[] position);


        //Properties with get and/or set ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public abstract double Size { get; set; }
        //Size of softsensor, radius for control point and line thickness for control line.

        public abstract ThresHoldTypeEnum ThresType { get; set; }
        //Threshold type of the soft sensor.

        public abstract double Threshold { get; set; }
        //Threshold value.

        public abstract bool EvalResult { get; }
        //Read only and only valid fater Evaluate, returns the status of the Control Point, true if OK and flase if NOK.

        public abstract double EvalDataValAvg { get; }
        //Read only and only valid after Evaluate, returns the appropiate average value used for evaluation of threshold.

        public abstract double EvalDataValMax { get; }
        //Read only and only valid after Evaluate, Returns the appropiate max value used for the evaluation of the threshold.


        public abstract double EvalDataValMin { get; }
        //Read only and only valid after Evaluate, Returns the appropiate min value used for the evaluation of the threshold.

        public abstract SoftSensorTypeEnum SoftSensorType { get; }
        //Read Onlym set in constructor, Sensor Type.


    }//End of SoftSensor


}//End of namespace
