using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace LabelingTest
{

    public partial class Form1 : Form
    {
        private const int ROISide = 801;
        private const int ROIHalfSide = 400;
        private const int ROIcenterRow = 480;
        private const int ROIcenterColumn = 640;
        
        private const int TotalCountLowF = 150;    	// F means filter // if any error change to 50 v2
        private const int DarkThresholdValue = 10;
        private const int DarkCountLowF = 500;	        // F means filter
        private const int BrightThresholdValue = 200;
        private const int BrightCountLowF = 25;       	// F means filter // if any error change to 50 v3
        
        private struct BrightDarkThreshold
        {
            public byte BrightThreshold;
            public byte DarkThreshold;
        }
        class LabelOBJ<T>
        {
            public LabelOBJ<T> previous;
            public LabelOBJ<T> next;

            public T OBJIndex;
            public T OBJOriIndex;
            public BrightDarkThreshold BDThresholdF;
            public long BCount;
            public long BCountF;
            public long DCount;
            public long DCountF;
            public long TCount;
            public long TCountF;
            public bool status;

            public LabelOBJ(T index, T oriIndex)
            {
                previous = null;
                next = null;
                OBJIndex = index;
                OBJOriIndex = oriIndex;
                BDThresholdF.BrightThreshold = BrightThresholdValue;
                BDThresholdF.DarkThreshold = DarkThresholdValue;
                BCount = 0;
                BCountF = BrightCountLowF;
                DCount = 0;
                DCountF = DarkCountLowF;
                TCount = 0;
                TCountF = TotalCountLowF;
                status = true;
            }
        }
        
        abstract class myLinkedList<T>
        {
            public int Count;
            public LabelOBJ<T> First;
            public LabelOBJ<T> Last;
            abstract public void add2First(T index, T oriIndex);
            abstract public void add2Last(T index, T oriIndex);
            abstract public bool add2Before(T dest_index, T index, T oriIndex);
            abstract public bool add2After(T dest_index, T index, T oriIndex);
            abstract public void RemoveFirst();
            abstract public void RemoveLast();
            abstract public void Remove(T index);
            abstract public void RemoveAll();
            abstract public void PushData(LabelOBJ<T> obj, byte color);
            abstract public LabelOBJ<T> FindOBJ(T index);
            abstract public LabelOBJ<T> FindOBJbyOri(T oriIndex);
        }
        
        private class LabelList<T> : myLinkedList<T>
        {
            public override void add2First(T index, T oriIndex)
            {
                LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                if (Count == 0)
                    Last = newOBJ;
                else
                    newOBJ.next = First;
                First = newOBJ;
                Count++;          
            }
            public override void add2Last(T index, T oriIndex)
            {
                LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                if (Count == 0)
                    First = newOBJ;
                else
                    Last.next = newOBJ;
                newOBJ.previous = Last;
                Last = newOBJ;
                Count++;  
            }
            public override bool add2Before(T dest_index, T index, T oriIndex)
            {
                LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                LabelOBJ<T> destOBJ = FindOBJ(dest_index);
                if (destOBJ == null)
                    return false;
                else
                {
                    newOBJ.previous = destOBJ.previous;
                    newOBJ.next = destOBJ;
                    (destOBJ.previous).next = newOBJ;
                    destOBJ.previous = newOBJ;
                }                
                Count++;
                return true;
            }
            public override bool add2After(T dest_index, T index, T oriIndex)
            {
                LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                LabelOBJ<T> destOBJ = FindOBJ(dest_index);
                if (destOBJ == null)
                    return false;
                else
                {
                    newOBJ.next = destOBJ.next;
                    newOBJ.previous = destOBJ;
                    (destOBJ.next).previous = newOBJ;
                    destOBJ.next = newOBJ;
                }
                Count++;
                return true;
            }
            public override void RemoveFirst()
            {
                if (Count == 0)
                    throw new IndexOutOfRangeException();
                else if (Count == 1)
                {
                    First = null;
                    Last = null;
                }
                else
                {
                    LabelOBJ<T> newOBJ = First.next;
                    First.next = null;
                    First = newOBJ;
                }
                Count--;
            }
            public override void RemoveLast()
            {
                if (Count == 0)
                    throw new IndexOutOfRangeException();
                else if (Count == 1)
                {
                    First = null;
                    Last = null;
                }
                else
                {
                    LabelOBJ<T> newOBJ = Last.previous;
                    newOBJ.next = null;
                    Last = newOBJ;
                }
                Count--;
            }
            public override void Remove(T index)
            {
                LabelOBJ<T> newOBJ = FindOBJ(index);

                if (newOBJ == null)
                    throw new IndexOutOfRangeException();
                else if (newOBJ == First)
                    RemoveFirst();
                else if (newOBJ == Last)
                    RemoveLast();
                else
                {
                    (newOBJ.previous).next = newOBJ.next;
                    (newOBJ.next).previous = newOBJ.previous;
                    Count--;
                }
            }
            public override void RemoveAll()
            {
                First = null;
                Last = null;
                Count = 0;
            }
            public override void PushData(LabelOBJ<T> obj, byte color)
            {
                obj.TCount++;
                if (obj.BDThresholdF.DarkThreshold >= color)
                    obj.DCount++;
                if (obj.BDThresholdF.BrightThreshold <= color)
                    obj.BCount++;
                if (obj.DCountF <= obj.DCount)
                    obj.status = false;
                else if (obj.BCountF <= obj.BCount)
                    obj.status = false;
                else if (obj.TCountF >= obj.TCount)
                    obj.status = false;
                else
                    obj.status = true;
            }
            public override LabelOBJ<T> FindOBJ(T index)
            {
                LabelOBJ<T> temp = First;
                while (temp != null)
                {
                    if (temp.OBJIndex.Equals(index))
                        return temp;
                    temp = temp.next;
                }
                return null;
            }
            public override LabelOBJ<T> FindOBJbyOri(T OriIndex)
            {
                LabelOBJ<T> temp = First;
                while (temp != null)
                {
                    if (temp.OBJOriIndex.Equals(OriIndex))
                        return temp;
                    temp = temp.next;
                }
                return null;
            }
        }
    

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> img = new Image<Gray, byte>("F:/Image/GolfBall/20150320/漆結塊5.bmp");
            //Image<Gray, Byte> img = new Image<Gray, byte>("F:/Jeremy/JeremyProgram/Golf defect/Golf Sample/油墨N.bmp");
            //Image<Gray, Byte> img = new Image<Gray, byte>("J:/Yuntech/Class/ComputerVision_Total/ComputerVisionHomework/ABC.jpg");
            //Image<Gray, Byte> img = new Image<Gray, byte>("C:/Users/EL404/Desktop/delete/flash.png");
            
            System.Diagnostics.Stopwatch swToProc = new System.Diagnostics.Stopwatch();

            swToProc.Reset();
            swToProc.Start();

            imgProc imgProcessing = new imgProc();
            imgProcessing.imgBuffer = img;
            imgProcessing.imageProc();
            Image<Gray, byte> ResultImg2 = imgProcessing.ResultImg2;
            Image<Gray, Int32> ResultImg1 = imgProcessing.ResultImg;
            /*
            imgProc(img);*/
            //imageBox1.Image = ResultImg1;
            imageBox2.Image = ResultImg2;


            swToProc.Stop();
            label1.Text = "processing time :" + swToProc.Elapsed.TotalMilliseconds.ToString();

        }
                
        public class imgProc
        {
            public Image<Gray, Byte> img;
            public Image<Gray, Int32> ResultImg;
            public Image<Gray, Byte> ResultImg2;
            public Image<Gray, Byte> imgBuffer;

            private const int ROISide = 801;
            private const int ROIHalfSide = 400;
            private const int ROIcenterRow = 480;
            private const int ROIcenterColumn = 640;

            private const int TotalCountLowF = 150;    	// F means filter // if any error change to 50 v2
            private const int DarkThresholdValue = 40;
            private const int DarkCountUpF = 500000;	        // F means filter
            private const int DarkCountLowF = 500;	        // F means filter
            private const int BrightThresholdValue = 200;
            private const int BrightCountUpF = 1000;
            private const int BrightCountLowF = 0;       	// F means filter // if any error change to 50 v3

            private struct BrightDarkThreshold
            {
                public byte BrightThreshold;
                public byte DarkThreshold;
            }

            class LabelOBJ<T>
            {
                public LabelOBJ<T> previous;
                public LabelOBJ<T> next;

                public T OBJIndex;
                public T OBJOriIndex;
                public BrightDarkThreshold BDThresholdF;
                public long BCount;
                public long BCountF;
                public long DCount;
                public long DCountF;
                public long TCount;
                public long TCountF;
                public bool status;

                public LabelOBJ(T index, T oriIndex)
                {
                    previous = null;
                    next = null;
                    OBJIndex = index;
                    OBJOriIndex = oriIndex;
                    BDThresholdF.BrightThreshold = BrightThresholdValue;
                    BDThresholdF.DarkThreshold = DarkThresholdValue;
                    BCount = 0;
                    BCountF = BrightCountLowF;
                    DCount = 0;
                    DCountF = DarkCountLowF;
                    TCount = 0;
                    TCountF = TotalCountLowF;
                    status = true;
                }
            }

            abstract class myLinkedList<T>
            {
                public int Count;
                public LabelOBJ<T> First;
                public LabelOBJ<T> Last;
                abstract public void add2First(T index, T oriIndex);
                abstract public void add2Last(T index, T oriIndex);
                abstract public bool add2Before(T dest_index, T index, T oriIndex);
                abstract public bool add2After(T dest_index, T index, T oriIndex);
                abstract public void RemoveFirst();
                abstract public void RemoveLast();
                abstract public void Remove(T index);
                abstract public void RemoveAll();
                abstract public void PushData(LabelOBJ<T> obj, byte color);
                abstract public LabelOBJ<T> FindOBJ(T index);
                abstract public LabelOBJ<T> FindOBJbyOri(T oriIndex);
            }
            private class LabelList<T> : myLinkedList<T>
            {
                public override void add2First(T index, T oriIndex)
                {
                    LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                    if (Count == 0)
                        Last = newOBJ;
                    else
                        newOBJ.next = First;
                    First = newOBJ;
                    Count++;
                }
                public override void add2Last(T index, T oriIndex)
                {
                    LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                    if (Count == 0)
                        First = newOBJ;
                    else
                        Last.next = newOBJ;
                    newOBJ.previous = Last;
                    Last = newOBJ;
                    Count++;
                }
                public override bool add2Before(T dest_index, T index, T oriIndex)
                {
                    LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                    LabelOBJ<T> destOBJ = FindOBJ(dest_index);
                    if (destOBJ == null)
                        return false;
                    else
                    {
                        newOBJ.previous = destOBJ.previous;
                        newOBJ.next = destOBJ;
                        (destOBJ.previous).next = newOBJ;
                        destOBJ.previous = newOBJ;
                    }
                    Count++;
                    return true;
                }
                public override bool add2After(T dest_index, T index, T oriIndex)
                {
                    LabelOBJ<T> newOBJ = new LabelOBJ<T>(index, oriIndex);
                    LabelOBJ<T> destOBJ = FindOBJ(dest_index);
                    if (destOBJ == null)
                        return false;
                    else
                    {
                        newOBJ.next = destOBJ.next;
                        newOBJ.previous = destOBJ;
                        (destOBJ.next).previous = newOBJ;
                        destOBJ.next = newOBJ;
                    }
                    Count++;
                    return true;
                }
                public override void RemoveFirst()
                {
                    if (Count == 0)
                        throw new IndexOutOfRangeException();
                    else if (Count == 1)
                    {
                        First = null;
                        Last = null;
                    }
                    else
                    {
                        LabelOBJ<T> newOBJ = First.next;
                        First.next = null;
                        First = newOBJ;
                    }
                    Count--;
                }
                public override void RemoveLast()
                {
                    if (Count == 0)
                        throw new IndexOutOfRangeException();
                    else if (Count == 1)
                    {
                        First = null;
                        Last = null;
                    }
                    else
                    {
                        LabelOBJ<T> newOBJ = Last.previous;
                        newOBJ.next = null;
                        Last = newOBJ;
                    }
                    Count--;
                }
                public override void Remove(T index)
                {
                    LabelOBJ<T> newOBJ = FindOBJ(index);

                    if (newOBJ == null)
                        throw new IndexOutOfRangeException();
                    else if (newOBJ == First)
                        RemoveFirst();
                    else if (newOBJ == Last)
                        RemoveLast();
                    else
                    {
                        (newOBJ.previous).next = newOBJ.next;
                        (newOBJ.next).previous = newOBJ.previous;
                        Count--;
                    }
                }
                public override void RemoveAll()
                {
                    First = null;
                    Last = null;
                    Count = 0;
                }
                public override void PushData(LabelOBJ<T> obj, byte color)
                {
                    obj.TCount++;
                    if (obj.BDThresholdF.DarkThreshold >= color)
                        obj.DCount++;
                    if (obj.BDThresholdF.BrightThreshold <= color)
                        obj.BCount++;
                    if (obj.DCountF <= obj.DCount)
                        obj.status = false;
                    else if (obj.BCountF <= obj.BCount)
                        obj.status = false;
                    else if (obj.TCountF >= obj.TCount)
                        obj.status = false;
                    else
                        obj.status = true;
                }
                public override LabelOBJ<T> FindOBJ(T index)
                {
                    LabelOBJ<T> temp = First;
                    while (temp != null)
                    {
                        if (temp.OBJIndex.Equals(index))
                            return temp;
                        temp = temp.next;
                    }
                    return null;
                }
                public override LabelOBJ<T> FindOBJbyOri(T OriIndex)
                {
                    LabelOBJ<T> temp = First;
                    while (temp != null)
                    {
                        if (temp.OBJOriIndex.Equals(OriIndex))
                            return temp;
                        temp = temp.next;
                    }
                    return null;
                }
            }

            private Image<Gray, Int32> sLabeling(Image<Gray, Byte> origImg, Image<Gray, Byte> img)
            {
                System.Diagnostics.Stopwatch swToProc1 = new System.Diagnostics.Stopwatch();

                swToProc1.Reset();
                swToProc1.Start();

                Image<Gray, Byte> OrigImg = origImg.Copy();
                int img_height = img.Size.Height;
                int img_width = img.Size.Width;

                Image<Gray, Int32> LabelImg = new Image<Gray, Int32>(img_width, img_height);

                //List<Int32> LabelArray = new List<Int32>();
                int[] LabelArray = new int[65535];

                // Step.1 Scan
                int markIndex = 1;
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        int mark = -1;
                        if (img.Data[y, x, 0] == 255)
                        {
                            for (int my = -1; my <= 0; my++)
                                for (int mx = -1; mx <= 1; mx++)
                                {
                                    if (my == 0 && mx >= 0) // over
                                        break;
                                    if (y == 0 && my == -1) // upper
                                        break;
                                    if (x == 0 && mx == -1) // left side
                                        continue;
                                    if (x == img_width - 1 && mx == 1) // right side
                                        break;
                                    //try
                                    //{

                                    if (LabelImg.Data[y + my, x + mx, 0] != 0)
                                    {
                                        if (mark == -1)
                                            mark = LabelImg.Data[y + my, x + mx, 0];
                                        else if (mark > LabelImg.Data[y + my, x + mx, 0])
                                        {
                                            LabelArray[mark] = LabelImg.Data[y + my, x + mx, 0];
                                            mark = LabelImg.Data[y + my, x + mx, 0];
                                        }
                                        else if (mark < LabelImg.Data[y + my, x + mx, 0])
                                            LabelArray[LabelImg.Data[y + my, x + mx, 0]] = mark;
                                    }
                                    //}
                                    //catch { }
                                }
                            if (mark == -1)
                            {
                                LabelImg.Data[y, x, 0] = markIndex;
                                LabelArray[LabelImg.Data[y, x, 0]] = markIndex++;
                            }
                            else
                                LabelImg.Data[y, x, 0] = mark;
                        }
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.1 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.2 classify
                for (int c = 1; LabelArray[c] != 0; c++)
                    LabelArray[c] = findSource(LabelArray, c);

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.2 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.3 arrange
                ArrayList LabelArrayOfArrange = new ArrayList();
                for (int c = 1; LabelArray[c] != 0; c++)
                    if (!LabelArrayOfArrange.Contains(LabelArray[c]))
                        LabelArrayOfArrange.Add(LabelArray[c]);
                LabelArrayOfArrange.Sort();

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.3 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.4 build label object and make color for LabelImg
                LabelList<int> myLabelList = new LabelList<int>();
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        if (LabelImg.Data[y, x, 0] != 0)
                        {
                            LabelImg.Data[y, x, 0] = LabelArray[LabelImg.Data[y, x, 0]];
                            int index = LabelArrayOfArrange.IndexOf(LabelImg.Data[y, x, 0]);
                            if (index != -1)
                            {
                                LabelOBJ<int> temp = myLabelList.FindOBJ(index);
                                if (temp == null)
                                    myLabelList.add2Last(index, LabelImg.Data[y, x, 0]);
                                else
                                    myLabelList.PushData(temp, OrigImg.Data[y, x, 0]);
                            }
                        }
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.4 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();
                
                // Step.5 show (in release, delete this)
                // D me
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        LabelOBJ<int> temp = myLabelList.FindOBJbyOri(LabelImg.Data[y, x, 0]);
                        if (temp != null && !temp.status)
                            LabelImg.Data[y, x, 0] = 0;
                        else if (temp != null)
                            LabelImg.Data[y, x, 0] = 255; // temp.OBJOriIndex;// LabelImg.Data[y, x, 0] * 100;
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.5 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                //imageBox1.Image = LabelImg;

                return LabelImg;
            }
/*
            private Image<Gray, Int32> sLabeling2(Image<Gray, Byte> origImg, Image<Gray, Byte> img)
            {
                System.Diagnostics.Stopwatch swToProc1 = new System.Diagnostics.Stopwatch();

                swToProc1.Reset();
                swToProc1.Start();

                Image<Gray, Byte> OrigImg = origImg.Clone();
                int img_height = img.Size.Height;
                int img_width = img.Size.Width;

                Image<Gray, Int32> LabelImg = new Image<Gray, Int32>(img_width, img_height);
                Image<Gray, bool> MarkImg = new Image<Gray, bool>(OrigImg.Size);

                // Step.1 Scan
                int markIndex = 1;
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        int mark = -1;
                        if (img.Data[y, x, 0] == 255)
                        {
                            for (int my = -1; my <= 0; my++)
                                for (int mx = -1; mx <= 1; mx++)
                                {
                                    if (my == 0 && mx >= 0) // over
                                        break;
                                    if (y == 0 && my == -1) // upper
                                        break;
                                    if (x == 0 && mx == -1) // left side
                                        continue;
                                    if (x == img_width - 1 && mx == 1) // right side
                                        break;
                                    //try
                                    //{

                                    if (LabelImg.Data[y + my, x + mx, 0] != 0)
                                    {
                                        if (mark == -1)
                                            mark = LabelImg.Data[y + my, x + mx, 0];
                                        else if (mark > LabelImg.Data[y + my, x + mx, 0])
                                        {
                                            LabelArray[mark] = LabelImg.Data[y + my, x + mx, 0];
                                            mark = LabelImg.Data[y + my, x + mx, 0];
                                        }
                                        else if (mark < LabelImg.Data[y + my, x + mx, 0])
                                            LabelArray[LabelImg.Data[y + my, x + mx, 0]] = mark;
                                    }
                                    //}
                                    //catch { }
                                }
                            if (mark == -1)
                            {
                                LabelImg.Data[y, x, 0] = markIndex;
                                LabelArray[LabelImg.Data[y, x, 0]] = markIndex++;
                            }
                            else
                                LabelImg.Data[y, x, 0] = mark;
                        }
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.1 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.2 classify
                for (int c = 1; LabelArray[c] != 0; c++)
                    LabelArray[c] = findSource(LabelArray, c);

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.2 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.3 arrange
                ArrayList LabelArrayOfArrange = new ArrayList();
                for (int c = 1; LabelArray[c] != 0; c++)
                    if (!LabelArrayOfArrange.Contains(LabelArray[c]))
                        LabelArrayOfArrange.Add(LabelArray[c]);
                LabelArrayOfArrange.Sort();

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.3 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.4 build label object and make color for LabelImg
                LabelList<int> myLabelList = new LabelList<int>();
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        if (LabelImg.Data[y, x, 0] != 0)
                        {
                            LabelImg.Data[y, x, 0] = LabelArray[LabelImg.Data[y, x, 0]];
                            int index = LabelArrayOfArrange.IndexOf(LabelImg.Data[y, x, 0]);
                            if (index != -1)
                            {
                                LabelOBJ<int> temp = myLabelList.FindOBJ(index);
                                if (temp == null)
                                    myLabelList.add2Last(index, LabelImg.Data[y, x, 0]);
                                else
                                    myLabelList.PushData(temp, OrigImg.Data[y, x, 0]);
                            }
                        }
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.4 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                swToProc1.Reset();
                swToProc1.Start();

                // Step.5 show (in release, delete this)
                // D me
                for (int y = 0; y < img_height; y++)
                    for (int x = 0; x < img_width; x++)
                    {
                        LabelOBJ<int> temp = myLabelList.FindOBJbyOri(LabelImg.Data[y, x, 0]);
                        if (temp != null && !temp.status)
                            LabelImg.Data[y, x, 0] = 0;
                        else if (temp != null)
                            LabelImg.Data[y, x, 0] = 255; // temp.OBJOriIndex;// LabelImg.Data[y, x, 0] * 100;
                    }

                swToProc1.Stop();

                Console.WriteLine("Proc.1 for Step.5 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

                //imageBox1.Image = LabelImg;

                return LabelImg;
            }
            */
            private int findSource(int[] array, int index)
            {
                if (array[index] == index)
                    return index;
                else
                    return findSource(array, array[index]);
            }

            public void imageProc()
            {
                img = imgBuffer.Clone();

                // Flip Image
                Image<Gray, byte> img_flip = img.Clone();
                //CvInvoke.cvFlip(img, img_flip, FLIP.NONE);
                                
                // Cut ROI
                Rectangle sROI = new Rectangle(ROIcenterColumn - ROIHalfSide, ROIcenterRow - ROIHalfSide, ROISide, ROISide);
                img_flip.ROI = sROI;
                //img_flip.Save("F:/Image/GolfBall/20150320/Test/1roi.bmp");
                
                // Sobel
                Image<Gray, float> SobelImage_X = img_flip.Sobel(1, 0, 3);
                Image<Gray, float> SobelImage_Y = img_flip.Sobel(0, 1, 3);
                // ABS
                SobelImage_X.AbsDiff(new Gray(0));
                SobelImage_Y.AbsDiff(new Gray(0));
                Image<Gray, float> SobelImage_16 = SobelImage_X + SobelImage_Y;
                double[] mins, maxs;
                Point[] minLoc, maxLoc;
                SobelImage_16.MinMax(out mins, out maxs, out minLoc, out maxLoc);
                Image<Gray, Byte> img_sobel = SobelImage_16.ConvertScale<byte>(255 / maxs[0], 0);
                img_sobel._ThresholdBinary(new Gray(20), new Gray(255));
                
                // Closing
                StructuringElementEx SElement = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
                Image<Gray, Byte> img_closing = img_sobel.MorphologyEx(SElement, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 1);
                img_closing = img_closing.MorphologyEx(SElement,Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN,1);
                /*
                System.Diagnostics.Stopwatch swToProc1 = new System.Diagnostics.Stopwatch();
                
                swToProc1.Reset();
                swToProc1.Start();*/
                Image<Gray, byte> img_brightLabel = GetBrightSide(img_flip);
                Image<Gray, byte> img_darkLabel = GetDarkSide(img);
                img_darkLabel.ROI = sROI;

                //img_brightLabel.Save("F:/Image/GolfBall/20150320/Test/0b.bmp");
                //img_darkLabel.Save("F:/Image/GolfBall/20150320/Test/0d.bmp");

                Image<Gray, byte> ResultImage = img_closing.Copy();
                //ResultImage.Save("F:/Image/GolfBall/20150320/Test/2closing.bmp");
                img_brightLabel = img_brightLabel.Dilate(5);
                img_darkLabel = img_darkLabel.Dilate(5);
                img_brightLabel = img_brightLabel + img_darkLabel;
                //img_brightLabel.Save("F:/Image/GolfBall/20150320/Test/3filter.bmp");
                ResultImage = ResultImage - img_brightLabel;
                ResultImage = ResultImage.MorphologyEx(SElement, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 3);
                ResultImage = GetBeFilterImgBySize(ResultImage);
                //ResultImage.Save("F:/Image/GolfBall/20150320/Test/4result.bmp");

                /*
                swToProc1.Stop();
                Console.WriteLine("Bright Region Processing Time :" + swToProc1.Elapsed.TotalMilliseconds.ToString());
                */
                ResultImg2 = ResultImage;

            }

            private Image<Gray, byte> GetBrightSide(Image<Gray, Byte> inputImg)
            {
                Image<Gray, byte> temp = inputImg.Clone();

                // Threshold
                temp._ThresholdBinary(new Gray(BrightThresholdValue), new Gray(255));

                // Connected Component and Draw
                Image<Gray, byte> resultImage = temp.CopyBlank();
                Gray gray = new Gray(255);

                using (var mem = new MemStorage())
                {
                    for (var contour = temp.FindContours(
                        CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                        RETR_TYPE.CV_RETR_LIST,
                        mem); contour != null; contour = contour.HNext)
                    {
                        if ((contour.Area < BrightCountUpF) && (contour.Area > BrightCountLowF))
                            resultImage.Draw(contour, gray, -1);
                    }
                }

                return resultImage;
            }
            private Image<Gray, byte> GetDarkSide(Image<Gray, Byte> inputImg)
            {
                Image<Gray, byte> temp = inputImg.Clone();

                // Threshold
                temp = temp.ThresholdBinaryInv(new Gray(DarkThresholdValue),new Gray(255));

                // Connected Component and Draw
                Image<Gray, byte> resultImage = temp.CopyBlank();
                Gray gray = new Gray(255);

                using (var mem = new MemStorage())
                {
                    for (var contour = temp.FindContours(
                        CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                        RETR_TYPE.CV_RETR_LIST,
                        mem); contour != null; contour = contour.HNext)
                    {
                        if ((contour.Area < DarkCountUpF) && (contour.Area > DarkCountLowF))
                            resultImage.Draw(contour, gray, -1);
                    }
                }

                return resultImage;
            }
            private Image<Gray, byte> GetBeFilterImgBySize(Image<Gray, byte> inputImg)
            {
                Image<Gray, byte> temp = inputImg.Clone();

                // Connected Component and Draw
                Image<Gray, byte> resultImage = temp.CopyBlank();
                Gray gray = new Gray(255);

                using (var mem = new MemStorage())
                {
                    for (var contour = temp.FindContours(
                        CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                        RETR_TYPE.CV_RETR_LIST,
                        mem); contour != null; contour = contour.HNext)
                    {
                        if (contour.Area > TotalCountLowF)
                            resultImage.Draw(contour, gray, -1);
                    }
                }

                return resultImage;
            }

            private Image<Gray, byte> FillHoles(Image<Gray, byte> image, int minArea, int maxArea)
            {
                var resultImage = image.CopyBlank();
                Gray gray = new Gray(255);

                using (var mem = new MemStorage())
                {
                    for (var contour = image.FindContours(
                        CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                        RETR_TYPE.CV_RETR_CCOMP,
                        mem); contour != null; contour = contour.HNext)
                    {
                        if ((contour.Area < maxArea) && (contour.Area > minArea))
                            resultImage.Draw(contour, gray, -1);
                    }
                }

                return resultImage;
            }
        }


        /*
        private unsafe Image<Gray, Byte> imgProc(Image<Gray, byte> img)
        {
            System.Diagnostics.Stopwatch swToProc1 = new System.Diagnostics.Stopwatch();

            swToProc1.Reset();
            swToProc1.Start();

            // Flip Image
            Image<Gray, byte> img_flip = new Image<Gray, Byte>(img.Size.Width, img.Size.Height);
            img_flip = img;
            //CvInvoke.cvFlip(img, img_flip, FLIP.NONE);
            // Cut ROI
            Rectangle sROI = new Rectangle(ROIcenterColumn - ROIHalfSide, ROIcenterRow - ROIHalfSide, ROISide, ROISide);
            img_flip.ROI = sROI;

            imageBox2.Image = img_flip;

            // Sobel
            Image<Gray, float> SobelImage_X = img_flip.Sobel(1, 0, 3);
            Image<Gray, float> SobelImage_Y = img_flip.Sobel(0, 1, 3);
            // ABS
            SobelImage_X.AbsDiff(new Gray(0));
            SobelImage_Y.AbsDiff(new Gray(0));
            Image<Gray, float> SobelImage_16 = SobelImage_X + SobelImage_Y;
            double[] mins, maxs;
            Point[] minLoc, maxLoc;
            SobelImage_16.MinMax(out mins, out maxs, out minLoc, out maxLoc);
            Image<Gray, Byte> img_sobel = SobelImage_16.ConvertScale<byte>(255 / maxs[0], 0);
            img_sobel._ThresholdBinary(new Gray(20), new Gray(255));



            // Closing
            //Image<Gray, Byte> img_closing = img_sobel.Erode(1);
            //img_closing = img_closing.Dilate(1);
            StructuringElementEx SElement = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
            Image<Gray, Byte> img_closing = img_sobel.MorphologyEx(SElement, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 5);
            img_closing = img_closing.Erode(1);
            img_closing = img_closing.Dilate(1);
            
            swToProc1.Stop();

            swToProc1.Reset();
            swToProc1.Start();

            Console.WriteLine("Proc.1 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            // Labeling
            //sLabeling(img_flip, img_closing);
            //Image<Bgr, Byte> img_Labeling = new Image<Bgr, Byte>(img_closing.Size.Width, img_closing.Size.Height);
            Image<Gray, Byte> img_Labeling = new Image<Gray, Byte>(img_closing.Size.Width, img_closing.Size.Height);
            sLabeling(img_flip, img_closing);

            swToProc1.Stop();

            Console.WriteLine("Proc.1 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            /*
            Contour<Point> contours = img_closing.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL);
            UInt64 c = 0;
            for (; contours != null; contours = contours.HNext)
            {
                c += LabelColorOrder;
                if (c > MaxLabelingCounter)
                    return null;
                Bgr ext_color = new Bgr((c % 255) & 255, ((c >> 8) % 255) & 255, (c >> 16) & 255);
                img_Labeling.Draw(contours, ext_color, 1);
            }
            */
            // Select Region
            //Image<Gray, Byte> img_Selected = ref sImageProcessing(img_flip.Ptr, img_Labeling.Ptr);
            
            /*
            BrightDarkThreshold BDThreshold = new BrightDarkThreshold();
            BDThreshold.BrightThreshold = BrightThresholdValue;
            BDThreshold.DarkThreshold = DarkThresholdValue;
            LabelObject* LabelOBJhead = stackalloc LabelObject[1];
	        for (int y=0;y<img_Labeling.Size.Height;y++){
		        for (int x=0;x<img_Labeling.Size.Width;x++){
			        if ( (img_Labeling.Data[y,x,0]!=0) ||	
				        (img_Labeling.Data[y,x,1]!=0) || 
				        (img_Labeling.Data[y,x,2]!=0) ){
					        MCvScalar OBJcolor= new MCvScalar(img_Labeling.Data[y,x,0],
						        img_Labeling.Data[y,x,1],
						        img_Labeling.Data[y,x,2]);
                            addLabelObj(LabelOBJhead, img_flip.Data[y,x,0], BDThreshold, OBJcolor);
			        }
		        }
	        }
            for (int y = 0; y < img_Labeling.Size.Height; y++)
            {
                for (int x = 0; x < img_Labeling.Size.Width; x++)
                {
                    if ((img_Labeling.Data[y, x, 0] != 0) ||
                        (img_Labeling.Data[y, x, 1] != 0) ||
                        (img_Labeling.Data[y, x, 2] != 0))
                    {
                        MCvScalar OBJcolor = new MCvScalar(img_Labeling.Data[y, x, 0],
                                img_Labeling.Data[y, x, 1],
                                img_Labeling.Data[y, x, 2]);
                        LabelObject* temp = searchObj(LabelOBJhead, OBJcolor);
                        if (temp->BrightCount >= BrightCountLowF)
                            img_Selected.Data[y, x, 0] = 0;
                        else if (temp->DarkCount >= DarkCountLowF)
                            img_Selected.Data[y, x, 0] = 0;
                        else if (temp->TotalCount <= TotalCountLowF)
                            img_Selected.Data[y, x, 0] = 0;
                        else
                            img_Selected.Data[y, x, 0] = 255;
                    }
                }
            }*/
        /*
            //imageBox1.Image = img_Labeling;

            return null;
        }*/
        /*
        unsafe private void addLabelObj(LabelObject* first, byte pixelValue, BrightDarkThreshold BDT, MCvScalar OBJColor)
        {
            LabelObject* temp = first;

            if (temp->TotalCount == 0)
            {
                temp->ObjColor.v0 = OBJColor.v0;
                temp->ObjColor.v1 = OBJColor.v1;
                temp->ObjColor.v2 = OBJColor.v2;
                temp->ObjColor.v3 = OBJColor.v3;
                temp->BrightThreshold = BDT.BrightThreshold;
                temp->DarkThreshold = BDT.DarkThreshold;
                if (pixelValue >= temp->BrightThreshold)
                    temp->BrightCount++;
                else if (pixelValue < temp->DarkThreshold)
                    temp->DarkCount++;
                temp->TotalCount++;
                temp->nextOBJ = null;
            }
            else
            {
                bool breakFlag = false;
                while (temp->nextOBJ != null)
                {
                    if (temp->ObjColor.v0 == OBJColor.v0 &&
                        temp->ObjColor.v1 == OBJColor.v1 &&
                        temp->ObjColor.v2 == OBJColor.v2 &&
                        temp->ObjColor.v3 == OBJColor.v3)
                    {
                        if (pixelValue >= temp->BrightThreshold)
                            temp->BrightCount++;
                        else if (pixelValue < temp->DarkThreshold)
                            temp->DarkCount++;
                        temp->TotalCount++;
                        breakFlag = true;
                        break;
                    }
                    else
                        temp = temp->nextOBJ;
                }
                if (!breakFlag)
                {
                    LabelObject* newOBJ = stackalloc LabelObject[1];
                    newOBJ->ObjColor.v0 = OBJColor.v0;
                    newOBJ->ObjColor.v1 = OBJColor.v1;
                    newOBJ->ObjColor.v2 = OBJColor.v2;
                    newOBJ->ObjColor.v3 = OBJColor.v3;
                    newOBJ->BrightThreshold = BDT.BrightThreshold;
                    newOBJ->DarkThreshold = BDT.DarkThreshold;
                    if (pixelValue >= newOBJ->BrightThreshold)
                        newOBJ->BrightCount++;
                    else if (pixelValue < newOBJ->DarkThreshold)
                        newOBJ->DarkCount++;
                    newOBJ->TotalCount++;
                    newOBJ->nextOBJ = null;
                    temp->nextOBJ = newOBJ;
                }
            }
        }
        unsafe private LabelObject* searchObj(LabelObject* first, MCvScalar OBJColor)
        {
            LabelObject* temp = first;
            while (temp != null)
            {
                if (temp->ObjColor.v0 == OBJColor.v0 &&
                    temp->ObjColor.v1 == OBJColor.v1 &&
                    temp->ObjColor.v2 == OBJColor.v2 &&
                    temp->ObjColor.v3 == OBJColor.v3)
                {
                    return temp;
                }
                else
                    temp = temp->nextOBJ;
            }
            return null;

        }*/
    /*
        
        private Image<Gray,Int32> sLabeling(Image<Gray, Byte> origImg, Image<Gray, Byte> img)
        {
            System.Diagnostics.Stopwatch swToProc1 = new System.Diagnostics.Stopwatch();

            swToProc1.Reset();
            swToProc1.Start();

            Image<Gray, Byte> OrigImg = origImg.Copy();
            int img_height = img.Size.Height;
            int img_width = img.Size.Width;
                                    
            Image<Gray, Int32> LabelImg = new Image<Gray, Int32>(img_width, img_height);

            //List<Int32> LabelArray = new List<Int32>();
            int[] LabelArray = new int[65535];
            
            // Step.1 Scan
            int markIndex = 1;
            for (int y = 0; y < img_height; y++)
                for (int x = 0; x < img_width; x++)
                {
                    int mark = -1;
                    if (img.Data[y, x, 0] == 255)
                    {
                        for (int my = -1; my <= 0; my++)
                            for (int mx = -1; mx <= 1; mx++)
                            {
                                if (my == 0 && mx >= 0) // over
                                    break;
                                if (y == 0 && my == -1) // upper
                                    break;
                                if (x == 0 && mx == -1) // left side
                                    continue;
                                if (x == img_width - 1 && mx == 1) // right side
                                    break;
                                //try
                                //{
                                
                                if (LabelImg.Data[y + my, x + mx, 0] != 0)
                                {
                                    if (mark == -1)
                                        mark = LabelImg.Data[y + my, x + mx, 0];
                                    else if (mark > LabelImg.Data[y + my, x + mx, 0])
                                    {
                                        LabelArray[mark] = LabelImg.Data[y + my, x + mx, 0];
                                        mark = LabelImg.Data[y + my, x + mx, 0];
                                    }
                                    else if (mark < LabelImg.Data[y + my, x + mx, 0])
                                        LabelArray[LabelImg.Data[y + my, x + mx, 0]] = mark;
                                }
                                //}
                                //catch { }
                            }
                        if (mark == -1)
                        {
                            LabelImg.Data[y, x, 0] = markIndex;
                            LabelArray[LabelImg.Data[y, x, 0]] = markIndex++;
                        }
                        else
                            LabelImg.Data[y, x, 0] = mark;
                    }
                }

            swToProc1.Stop();

            Console.WriteLine("Proc.1 for Step.1 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            swToProc1.Reset();
            swToProc1.Start();

            // Step.2 classify
            for (int c = 1; LabelArray[c] != 0; c++)
                LabelArray[c] = findSource(LabelArray, c);

            swToProc1.Stop();

            Console.WriteLine("Proc.1 for Step.2 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            swToProc1.Reset();
            swToProc1.Start();

            // Step.3 arrange
            ArrayList LabelArrayOfArrange = new ArrayList();
            for (int c = 1; LabelArray[c] != 0; c++)
                if (!LabelArrayOfArrange.Contains(LabelArray[c]))
                    LabelArrayOfArrange.Add(LabelArray[c]);
            LabelArrayOfArrange.Sort();

            swToProc1.Stop();

            Console.WriteLine("Proc.1 for Step.3 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            swToProc1.Reset();
            swToProc1.Start();

            // Step.4 build label object and make color for LabelImg
            LabelList<int> myLabelList = new LabelList<int>();
            for (int y = 0; y < img_height; y++)
                for (int x = 0; x < img_width; x++)
                {
                    if (LabelImg.Data[y, x, 0] != 0)
                    {
                        LabelImg.Data[y, x, 0] = LabelArray[LabelImg.Data[y, x, 0]];
                        int index = LabelArrayOfArrange.IndexOf(LabelImg.Data[y, x, 0]);
                        if (index != -1)
                        {
                            LabelOBJ<int> temp = myLabelList.FindOBJ(index);
                            if (temp == null)
                                myLabelList.add2Last(index, LabelImg.Data[y, x, 0]);
                            else
                                myLabelList.PushData(temp, OrigImg.Data[y, x, 0]);
                        }
                    }
                }

            swToProc1.Stop();

            Console.WriteLine("Proc.1 for Step.4 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            swToProc1.Reset();
            swToProc1.Start();

            /*
            // Step.5 show (in release, delete this)
            // D me
            for (int y = 0; y < img_height; y++)
                for (int x = 0; x < img_width; x++)
                {
                    LabelOBJ<int> temp = myLabelList.FindOBJbyOri(LabelImg.Data[y, x, 0]);
                    if (temp != null && !temp.status)
                        LabelImg.Data[y, x, 0] = 0;
                    else if (temp != null)
                        LabelImg.Data[y, x, 0] = 255; // temp.OBJOriIndex;// LabelImg.Data[y, x, 0] * 100;
                }*/
        /*
            swToProc1.Stop();

            Console.WriteLine("Proc.1 for Step.5 Time = {0:f} ms\n", swToProc1.Elapsed.TotalMilliseconds.ToString());

            imageBox1.Image = LabelImg;

            return LabelImg;
        }*/
        
        /*
        private unsafe void sLabeling(Image<Gray, Byte> OrigImg, Image<Gray, Byte> img)
        {
            int img_height = img.Size.Height;
            int img_width = img.Size.Width;

            Int32[] LabelArray = new Int32[641601]; // 801* 801

            Image<Gray, Int32> LabelImg = new Image<Gray, Int32>(img_width, img_height);
            Int32 LabelIndex = 1;
            for (int y = 0; y < img_height; y++)
                for (int x = 0; x < img_width; x++)
                {
                    if (img.Data[y, x, 0] == 255)
                    {
                        if (y == 0 && x == 0)
                        {
                            LabelImg.Data[y, x, 0] = LabelIndex;
                            LabelArray[LabelIndex] = LabelIndex;
                            LabelIndex++;
                        }
                        else if (y == 0)
                        {
                            if (LabelImg.Data[y, x - 1, 0] != 0)
                                LabelImg.Data[y, x, 0] = LabelImg.Data[y, x - 1, 0];
                            else
                            {
                                LabelImg.Data[y, x, 0] = LabelIndex;
                                LabelArray[LabelIndex] = LabelIndex;
                                LabelIndex++;
                            }
                        }
                        else if (x == 0)
                        {
                            Int32 min = 641601;
                            if (LabelImg.Data[y - 1, x, 0] != 0)
                            {
                                min = LabelImg.Data[y - 1, x, 0];
                                if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                {
                                    if (min > LabelImg.Data[y - 1, x + 1, 0])
                                    {
                                        if (LabelArray[min] > LabelImg.Data[y - 1, x + 1, 0])
                                            LabelArray[min] = LabelImg.Data[y - 1, x + 1, 0];
                                        min = LabelImg.Data[y - 1, x + 1, 0];
                                    }
                                    else
                                        LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                }
                            }
                            else
                            {
                                if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                    min = LabelImg.Data[y - 1, x + 1, 0];
                            }

                            if (min == 641601)
                            {
                                LabelImg.Data[y, x, 0] = LabelIndex;
                                LabelArray[LabelIndex] = LabelIndex;
                                LabelIndex++;
                            }
                            else
                                LabelImg.Data[y, x, 0] = min;
                        }
                        else if (x == img_width - 1)
                        {
                            Int32 min = 641601;
                            if (LabelImg.Data[y - 1, x - 1, 0] != 0)
                            {
                                if (LabelImg.Data[y - 1, x, 0] != 0)
                                {
                                    if (LabelImg.Data[y, x - 1, 0] != 0)
                                    {
                                        min = Math.Min(Math.Min(LabelImg.Data[y - 1, x - 1, 0], LabelImg.Data[y - 1, x, 0]), LabelImg.Data[y, x - 1, 0]);
                                        if (LabelArray[LabelImg.Data[y - 1, x - 1, 0]] > min)
                                            LabelArray[LabelImg.Data[y - 1, x - 1, 0]] = min;
                                        if (LabelArray[LabelImg.Data[y - 1, x, 0]] > min)
                                            LabelArray[LabelImg.Data[y - 1, x, 0]] = min;
                                        if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                            LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                    }
                                    else
                                    {
                                        if (LabelImg.Data[y - 1, x - 1, 0] > LabelImg.Data[y - 1, x, 0])
                                        {
                                            if (LabelArray[LabelImg.Data[y - 1, x - 1, 0]] > LabelImg.Data[y - 1, x, 0])
                                                LabelArray[LabelImg.Data[y - 1, x - 1, 0]] = LabelImg.Data[y - 1, x, 0];
                                        }
                                        else
                                        {
                                            if (LabelArray[LabelImg.Data[y - 1, x, 0]] > LabelImg.Data[y - 1, x - 1, 0])
                                                LabelArray[LabelImg.Data[y - 1, x, 0]] = LabelImg.Data[y - 1, x - 1, 0];
                                        }
                                    }
                                }
                                else
                                {
                                    if (LabelImg.Data[y, x - 1, 0] != 0)
                                    {
                                        if (min > LabelImg.Data[y, x - 1, 0])
                                        {
                                            if (LabelArray[min] > LabelImg.Data[y, x - 1, 0])
                                                LabelArray[min] = LabelImg.Data[y, x - 1, 0];
                                            min = LabelImg.Data[y, x - 1, 0];
                                        }
                                        else
                                        {
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LabelImg.Data[y - 1, x, 0] != 0)
                                {
                                    min = LabelImg.Data[y - 1, x, 0];
                                    if (LabelImg.Data[y, x - 1, 0] != 0)
                                    {
                                        if (min > LabelImg.Data[y, x - 1, 0])
                                        {
                                            if (LabelArray[min] > LabelImg.Data[y, x - 1, 0])
                                                LabelArray[min] = LabelImg.Data[y, x - 1, 0];
                                            min = LabelImg.Data[y, x - 1, 0];
                                        }
                                        else
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                    }
                                }
                                else
                                {
                                    if (LabelImg.Data[y, x - 1, 0] != 0)
                                    {
                                        min = LabelImg.Data[y, x - 1, 0];
                                    }
                                }
                            }

                            if (min == 641601)
                            {
                                LabelImg.Data[y, x, 0] = LabelIndex;
                                LabelArray[LabelIndex] = LabelIndex;
                                LabelIndex++;
                            }
                            else
                                LabelImg.Data[y, x, 0] = min;
                        }
                        else
                        {
                            Int32 min = 641601;
                            if (LabelImg.Data[y - 1, x - 1, 0] != 0)
                            {
                                if (LabelImg.Data[y - 1, x, 0] != 0)
                                {
                                    if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            min = Math.Min(Math.Min(Math.Min(LabelImg.Data[y - 1, x - 1, 0], LabelImg.Data[y - 1, x, 0]),
                                                LabelImg.Data[y - 1, x + 1, 0]), LabelImg.Data[y, x - 1, 0]);
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                        }
                                        else
                                        {
                                            min = Math.Min(Math.Min(LabelImg.Data[y - 1, x - 1, 0], LabelImg.Data[y - 1, x, 0]), LabelImg.Data[y - 1, x + 1, 0]);
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                        }
                                    }
                                    else
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            min = Math.Min(Math.Min(LabelImg.Data[y - 1, x - 1, 0], LabelImg.Data[y - 1, x, 0]), LabelImg.Data[y, x - 1, 0]);
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                        }
                                        else
                                        {
                                            if (LabelImg.Data[y - 1, x, 0] > LabelImg.Data[y - 1, x - 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] > LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x, 0]] = LabelImg.Data[y - 1, x - 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] < LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x - 1, 0]] = LabelImg.Data[y - 1, x, 0];
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            min = Math.Min(Math.Min(LabelImg.Data[y - 1, x - 1, 0], LabelImg.Data[y - 1, x + 1, 0]), LabelImg.Data[y, x - 1, 0]);
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                        }
                                        else
                                        {
                                            if (LabelImg.Data[y - 1, x + 1, 0] > LabelImg.Data[y - 1, x - 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = LabelImg.Data[y - 1, x - 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] < LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x - 1, 0]] = LabelImg.Data[y - 1, x + 1, 0];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            if (LabelImg.Data[y, x - 1, 0] > LabelImg.Data[y - 1, x - 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y, x - 1, 0]] > LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y, x - 1, 0]] = LabelImg.Data[y - 1, x - 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y, x - 1, 0]] < LabelImg.Data[y - 1, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x - 1, 0]] = LabelImg.Data[y, x - 1, 0];
                                            }
                                        }
                                        else
                                        {
                                            min = LabelImg.Data[y - 1, x - 1, 0];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LabelImg.Data[y - 1, x, 0] != 0)
                                {
                                    if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            min = Math.Min(Math.Min(LabelImg.Data[y - 1, x, 0], LabelImg.Data[y - 1, x + 1, 0]), LabelImg.Data[y, x - 1, 0]);
                                            if (LabelArray[LabelImg.Data[y - 1, x, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = min;
                                            if (LabelArray[LabelImg.Data[y, x - 1, 0]] > min)
                                                LabelArray[LabelImg.Data[y, x - 1, 0]] = min;
                                        }
                                        else
                                        {
                                            if (LabelImg.Data[y - 1, x, 0] > LabelImg.Data[y - 1, x + 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] > LabelImg.Data[y - 1, x + 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x, 0]] = LabelImg.Data[y - 1, x + 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] < LabelImg.Data[y - 1, x + 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = LabelImg.Data[y - 1, x, 0];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            if (LabelImg.Data[y - 1, x, 0] > LabelImg.Data[y, x - 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] > LabelImg.Data[y, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x, 0]] = LabelImg.Data[y, x - 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x, 0]] < LabelImg.Data[y, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y, x - 1, 0]] = LabelImg.Data[y - 1, x, 0];
                                            }
                                        }
                                        else
                                        {
                                            min = LabelImg.Data[y - 1, x, 0];
                                        }
                                    }
                                }
                                else
                                {
                                    if (LabelImg.Data[y - 1, x + 1, 0] != 0)
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            if (LabelImg.Data[y - 1, x + 1, 0] > LabelImg.Data[y, x - 1, 0])
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] > LabelImg.Data[y, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y - 1, x + 1, 0]] = LabelImg.Data[y, x - 1, 0];
                                            }
                                            else
                                            {
                                                if (LabelArray[LabelImg.Data[y - 1, x + 1, 0]] < LabelImg.Data[y, x - 1, 0])
                                                    LabelArray[LabelImg.Data[y, x - 1, 0]] = LabelImg.Data[y - 1, x + 1, 0];
                                            }
                                        }
                                        else
                                        {
                                            min = LabelImg.Data[y - 1, x + 1, 0];
                                        }
                                    }
                                    else
                                    {
                                        if (LabelImg.Data[y, x - 1, 0] != 0)
                                        {
                                            min = LabelImg.Data[y, x - 1, 0];
                                        }
                                    }
                                }
                            }
                            if (min == 641601)
                            {
                                LabelImg.Data[y, x, 0] = LabelIndex;
                                LabelArray[LabelIndex] = LabelIndex;
                                LabelIndex++;
                            }
                            else
                                LabelImg.Data[y, x, 0] = min;
                        }
                    }
                }

            int index = 1;
            while (true)
            {
                LabelArray[index] = findSource(LabelArray, index);
                index++;
                if (LabelArray[index] == 0)
                    break;
            }

            int[] sArray = new int[1000];
            for (int i = 0; i < index; i++)
            {
                int min = sArray[index];
                for (int j = 0; j < 1000; j++)
                {
                    if (min>sArray[])
                    
                }
            }



            for (int y = 0; y < img_height; y++)
                for (int x = 0; x < img_width; x++)
                {

                }

        }*/
        /*
        private int findSource(int[] array, int index)
        {
            if (array[index] == index)
                return index;
            else
                return findSource(array, array[index]);
        }
        */
    }
}
