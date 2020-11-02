//Antonio Manilla Maldonado
//SMV
//Final Exam
//Implement the application as a console app with results printed un the console and a separate window for presentation of images. 

//Libraries
using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using OpenCvSharp;

//C:\Users\FinalDarkHunter\Dropbox\Exam_002\img

namespace Exam_002
{
    class Program
    {
        static void Main(string[] args)
        {
            //List of objects
            List<SoftSensor> SensorsP = new List<SoftSensor>();
            List<SoftSensor> SensorsL = new List<SoftSensor>();

            //Control Points inside the part. 
            //CP1 = (985,215); CP2 = (1045,240); CP3 = (850,345); CP4 = (705,425); CP5 = (420,475); CP6 = (450,435);
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 985, 215 }, 6, 80));
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 1045, 240 }, 6, 80));
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 850, 345 }, 6, 80));
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 705, 425 }, 6, 80));
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 420, 475 }, 6, 80));
            SensorsP.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Point, ThresHoldTypeEnum.Bright, new double[] { 450, 535 }, 6, 80));

            //Control Lines outside the part 
            //CL1 = (980,145,1120,160); CL2 = (685,530,1105,270); CL3 = (560,410,915,235); CL4 = (320,440,350,665);
            SensorsL.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Line, ThresHoldTypeEnum.Dark, new double[] { 980, 145, 1120, 160 }, 2, 80));
            SensorsL.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Line, ThresHoldTypeEnum.Dark, new double[] { 685, 530, 1105, 270 }, 2, 80));
            SensorsL.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Line, ThresHoldTypeEnum.Dark, new double[] { 560, 410, 915, 235 }, 2, 80));
            SensorsL.Add(SoftSensor.CreateSoftSensor(SoftSensorTypeEnum.Line, ThresHoldTypeEnum.Dark, new double[] { 320, 440, 350, 665 }, 2, 80));

            //Obtaining the Path of the pictures
            Console.WriteLine("input the file directory");
            string imagePath = Console.ReadLine();
            string[] fileNames = Directory.GetFiles(imagePath);


            foreach (string fileName in fileNames)
            {
                string[] splitPath = fileName.Split('\\');
                string displayName = splitPath[splitPath.Length - 1];

                Mat image = Cv2.ImRead(fileName,ImreadModes.Grayscale);
                Mat displayImage = image.CvtColor(ColorConversionCodes.GRAY2BGR);

                bool imgPass = true;
                
                //Evaluate all softsensors
                foreach (SoftSensor sensP in SensorsP)
                {
                    sensP.Evaluate(image);
                    sensP.DrawResult(displayImage);

                    if (imgPass)
                    {
                        imgPass = sensP.EvalResult;
                    }

                }//End of foreach2

                foreach (SoftSensor sensL in SensorsL)
                {
                    sensL.Evaluate(image);
                    sensL.DrawResult(displayImage);

                    if (imgPass)
                    {
                        imgPass = sensL.EvalResult;
                    }

                }//End of foreach3


                //Displays image name in image
                Cv2.PutText(displayImage, displayName, new Point(20,30), HersheyFonts.Italic, 1.0, imgPass?Scalar.Green:Scalar.Red, 2);

                //Display result in console
                Console.WriteLine("Image: " + displayName + ", " + (imgPass?"OK":"NOK"));

                //Displays in the consoles the obtained values. Average of each Control Point and the Max and Min value of this en each picture.
                double[] valOutput = new double[10];
                int countP = 0;
                int countL = 0;
                double tempMax = 0;
                double tempMin = 0;
                int softsPNo = 1;
                int softsLNo = 1;

                foreach (SoftSensor sensP in SensorsP)
                {
                    StringBuilder resultsLine = new StringBuilder("SoftSensor no: " + softsPNo.ToString());
                    resultsLine.AppendFormat(", {0}, {1}", sensP.SoftSensorType.ToString(), sensP.EvalResult ? "OK" : "NOK");
                    resultsLine.AppendFormat(", {0} {1}", sensP.EvalDataValAvg.ToString(), sensP.ThresType == ThresHoldTypeEnum.Bright ? ">" : "<");
                    resultsLine.AppendFormat(" {0}", sensP.Threshold.ToString());


                    softsPNo += 1;

                    
                    valOutput[countP] = sensP.EvalDataValAvg;

                    countP += 1;

                    if (countP > 5)
                    {
                        tempMax = valOutput.Max();
                        tempMin = valOutput.Min();

                        //resultsLine.AppendFormat(" {0}", sens.Threshold.ToString());
                        resultsLine.AppendFormat("\nMax: {0}, Min: {1}", tempMax.ToString(), tempMin.ToString());
                    }
                    

                    //Print all the results, merging all the strings in one.
                    Console.WriteLine(resultsLine);


                }//End of foreach4
 

                foreach (SoftSensor sensL in SensorsL)
                {
                    StringBuilder resultsLine = new StringBuilder("SoftSensor no: " + softsLNo.ToString());
                    resultsLine.AppendFormat(", {0}, {1}", sensL.SoftSensorType.ToString(), sensL.EvalResult ? "OK" : "NOK");
                    resultsLine.AppendFormat(", {0} {1}", sensL.EvalDataValAvg.ToString(), sensL.ThresType == ThresHoldTypeEnum.Dark ? ">" : "<");
                    resultsLine.AppendFormat(" {0}", sensL.Threshold.ToString());

                    softsLNo += 1;


                    valOutput[countL] = sensL.EvalDataValAvg;

                    countL += 1;

                    if (countL > 3)
                    {
                        tempMax = valOutput.Max();
                        tempMin = valOutput.Min();

                        //resultsLine.AppendFormat(" {0}", sens.Threshold.ToString());
                        resultsLine.AppendFormat("\nMax: {0}, Min: {1}", tempMax.ToString(), tempMin.ToString());
                    }


                    //Print all the results, merging all the strings in one.
                    Console.WriteLine(resultsLine);


                }//End of foreach5
                Console.WriteLine();

                Cv2.ImShow("Results Display", displayImage);
                Cv2.WaitKey();

            }//End of foreach1

            Cv2.DestroyAllWindows();


        }//End of Main       


    }//End of class program


}//End of namespace Exam_002

