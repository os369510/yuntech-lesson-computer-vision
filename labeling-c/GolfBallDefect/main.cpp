#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <math.h>
#include <iostream>
#include <iomanip>
#include <windows.h>
#include <opencv2\core\core.hpp>
#include <opencv2\highgui\highgui.hpp>
#include <opencv2\imgproc\imgproc.hpp>
#include <opencv2\imgproc\imgproc_c.h>

#pragma region Define

#define MaxLabelingCounter 255*255*255
#define LabelColorOrder 255*255*255/1000

#define TotalCountF 150	// F means filter // if any error change to 50 v2
#define DarkThresholdValue 80
#define DarkCountF 500	// F means filter
#define BrightThresholdValue 200 
#define BrightCountF 25	// F means filter // if any error change to 50 v3

#define ROISide 801
#define ROIHalfSide 400
#define ROIcenterRow 533
#define ROIcenterYColumn 645

#pragma endregion Define

#pragma region Namesapce Section
using namespace std; 
#pragma endregion Namesapce Section

#pragma region LabelBuildingStruct

typedef struct BrightDarkThreshold{
	unsigned char BrightThreshold;
	unsigned char DarkThreshold;
}BDThres;

typedef struct LabelObject{
	CvScalar ObjColor;
	unsigned char BrightThreshold;
	unsigned long BrightCount;
	unsigned char DarkThreshold;
	unsigned long DarkCount;
	unsigned long TotalCount;
	LabelObject *nextOBJ;
}pixelBGR;
#pragma endregion LabelBuildingStruct
#pragma region Label Building Function

void addLabelObj(pixelBGR *first,unsigned char pixelValue,BDThres BDT,CvScalar OBJColor){
	pixelBGR *temp=first;
	if (temp->TotalCount==0){
		temp->ObjColor.val[0]=OBJColor.val[0];
		temp->ObjColor.val[1]=OBJColor.val[1];
		temp->ObjColor.val[2]=OBJColor.val[2];
		temp->ObjColor.val[3]=OBJColor.val[3];
		temp->BrightThreshold=BDT.BrightThreshold;
		temp->DarkThreshold=BDT.DarkThreshold;
		if (pixelValue>=temp->BrightThreshold)
			temp->BrightCount++;
		else if(pixelValue<temp->DarkThreshold)
			temp->DarkCount++;
		temp->TotalCount++;
		temp->nextOBJ=NULL;
	}
	else{
		bool breakFlag=0;
		while(temp->nextOBJ!=NULL){
			if (temp->ObjColor.val[0]==OBJColor.val[0] && 
				temp->ObjColor.val[1]==OBJColor.val[1] &&
				temp->ObjColor.val[2]==OBJColor.val[2] &&
				temp->ObjColor.val[3]==OBJColor.val[3] ){
					if (pixelValue>=temp->BrightThreshold)
						temp->BrightCount++;
					else if(pixelValue<temp->DarkThreshold)
						temp->DarkCount++;
					temp->TotalCount++;
					breakFlag=1;
					break;
			}
			else 
				temp=temp->nextOBJ;			
		}
		if (!breakFlag){
			pixelBGR *newOBJ;
			newOBJ=(pixelBGR*)calloc(1,sizeof(pixelBGR));
			newOBJ->ObjColor.val[0]=OBJColor.val[0];
			newOBJ->ObjColor.val[1]=OBJColor.val[1];
			newOBJ->ObjColor.val[2]=OBJColor.val[2];
			newOBJ->ObjColor.val[3]=OBJColor.val[3];
			newOBJ->BrightThreshold=BDT.BrightThreshold;
			newOBJ->DarkThreshold=BDT.DarkThreshold;
			if (pixelValue>=newOBJ->BrightThreshold)
				newOBJ->BrightCount++;
			else if(pixelValue<newOBJ->DarkThreshold)
				newOBJ->DarkCount++;
			newOBJ->TotalCount++;
			newOBJ->nextOBJ=NULL;
			temp->nextOBJ=newOBJ;
		}
	}				
}

void freeLabelObj(pixelBGR *first){
	pixelBGR *temp1=first,*temp2;
	temp2=(pixelBGR*)calloc(1,sizeof(pixelBGR));
	while(temp1!=NULL){
		temp2=temp1;
		temp1=temp1->nextOBJ;
		free(temp2);
	}
	free(temp1);
}

pixelBGR* searchObj(pixelBGR *first,CvScalar OBJColor){
	pixelBGR *temp=first;
	while(temp!=NULL){
		if (temp->ObjColor.val[0]==OBJColor.val[0] && 
			temp->ObjColor.val[1]==OBJColor.val[1] &&
			temp->ObjColor.val[2]==OBJColor.val[2] &&
			temp->ObjColor.val[3]==OBJColor.val[3] ){
				return temp;
		}
		else
			temp=temp->nextOBJ;
	}
	return NULL;

}
#pragma endregion Label Building Function

void main()
{
#pragma region (Input:Non,Output:Non)
	// Initialize
	LARGE_INTEGER startTime,endTime,freq;	// process time flag and cpu frequency
	double TotalTime=0,loadTime=0,ROITime=0,preprocessTime=0,SobelTime=0,LabelingTime=0,
		ClosingTime=0,SelectLabelingTime=0;
	QueryPerformanceFrequency(&freq);		// Get CPU freq.
#pragma endregion Initialize
#pragma region (Input:Non,Output:inputImage_RGB)
	// Loading Image
	QueryPerformanceCounter(&startTime);
	//IplImage* inputImage_RGB=cvLoadImage("G:/Jeremy/JeremyProgram/Golf defect/Golf Sample/¤ò®hN.bmp");		// OK
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/ªo¾¥N.bmp");		// OK, have noise of character 
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/ªo¾¥P.bmp");		// false alarm, have defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¯}¥ÖN.bmp");		// OK
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¯}¥ÖP.bmp");		// OK (By v3),false alarm, have defect(By v2)
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¯}¬}N.bmp");		// false alarm, have noise of character
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¯}¬}P.bmp");		//*OK, have noise of character
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¶ÀÂIN.bmp");		//*True alarm, have other defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¶ÀÂIP.bmp");		//*OK, have noise of character
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¸}¬[²ªN.bmp");		//*True alarm v2
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/¸}¬[²ªP.bmp");		//*OK, have other defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£¨í¶ËN.bmp");		// OK
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£¨í¶ËP.bmp");		// false alarm, have defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£µ²¶ô50N.bmp");	//*OK, have defect and noise of character
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£µ²¶ô50N-2.bmp");	//*OK, have defect and noise of char.,light,edge
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£µ²¶ô50P.bmp");	// OK, but have other defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/º£µ²¶ô50P-2.bmp"); // OK, but have other defect v2
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/»s·lN.bmp");		//*True alarm, need to focus Radio between Bright and Dark
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/»s·lP.bmp");		// OK, but have other defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/Äé¥WN.bmp");		// OK
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/Äé¥WP.bmp");		// false alarm, have defect
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/ÄéÂIµõN.bmp");		// OK v3
	//IplImage* inputImage_RGB=cvLoadImage("D:/GolfDefect/Golf Sample/ÄéÂIµõP.bmp");		// false alarm, have defect
	IplImage* inputImage_RGB=cvLoadImage("J:/GolfDefect/20150204/»s·l/NG/01.bmp");
	//IplImage* inputImage_RGB=cvLoadImage("E:/20150128/¤º§é10«×_expos2.387/º£µ²¶ô/NG/05.bmp");		// OK
	QueryPerformanceCounter(&endTime);
	loadTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+loadTime;
#pragma endregion Loading Image
#pragma region (Input:inputImage_RGB,Output:inputImage)
	// Cut ROI
	QueryPerformanceCounter(&startTime);
	CvRect ROIbyRect=cvRect(ROIcenterYColumn-ROIHalfSide,ROIcenterRow-ROIHalfSide,ROISide,ROISide);
	cvSetImageROI(inputImage_RGB,ROIbyRect);
	IplImage* inputImage=cvCreateImage(cvGetSize(inputImage_RGB),IPL_DEPTH_8U,1);
	cvCvtColor(inputImage_RGB,inputImage,CV_BGR2GRAY);
	QueryPerformanceCounter(&endTime);
	ROITime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+ROITime;
#pragma endregion Cut ROI
#pragma region (Input:inputImage,Output:preprocessImage)
	// Pre process
	QueryPerformanceCounter(&startTime);
	IplImage *preprocessImage=inputImage;//cvCreateImage(cvGetSize(inputImage),IPL_DEPTH_8U,1);
	//cvThreshold(inputImage,preprocessImage,DarkThresholdValue,255,CV_THRESH_TOZERO);
	
	QueryPerformanceCounter(&endTime);
	preprocessTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+preprocessTime;
#pragma endregion Preprocess
#pragma region (Input:preprocessImage,Output:SobelImage)
	// Sobel	
	QueryPerformanceCounter(&startTime);
	IplImage *SobelImage_16S_X=cvCreateImage(cvGetSize(preprocessImage),IPL_DEPTH_16S,1),
		*SobelImage_16S_Y=cvCreateImage(cvGetSize(preprocessImage),IPL_DEPTH_16S,1),
		*SobelImage_16S=cvCreateImage(cvGetSize(preprocessImage),IPL_DEPTH_16S,1),
		*SobelImage_8U=cvCreateImage(cvGetSize(preprocessImage),IPL_DEPTH_8U,1),
		*SobelImage=cvCreateImage(cvGetSize(preprocessImage),preprocessImage->depth,1);
	cvSobel(preprocessImage,SobelImage_16S_X,1,0,3);
	cvSobel(preprocessImage,SobelImage_16S_Y,0,1,3);
	cvAdd(SobelImage_16S_X,SobelImage_16S_Y,SobelImage_16S,0);
	cvConvertScaleAbs(SobelImage_16S,SobelImage_8U,1,0);
	cvThreshold(SobelImage_8U,SobelImage,20,255,CV_THRESH_BINARY);
	QueryPerformanceCounter(&endTime);
	SobelTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;	
	TotalTime=TotalTime+SobelTime;
#pragma endregion Sobel
#pragma region (Input:SobelImage,Output:ClosingImageTemp_dilate)
	// Closing
	QueryPerformanceCounter(&startTime);
	IplImage *ClosingImage = cvCreateImage(cvGetSize(SobelImage),IPL_DEPTH_8U,1),
		*ClosingImageTemp = cvCreateImage(cvGetSize(SobelImage),IPL_DEPTH_8U,1),
		*ClosingImageTemp_erode = cvCreateImage(cvGetSize(SobelImage),IPL_DEPTH_8U,1),
		*ClosingImageTemp_dilate = cvCreateImage(cvGetSize(SobelImage),IPL_DEPTH_8U,1);
	cvErode(SobelImage,ClosingImageTemp_erode,NULL,1);
	cvDilate(ClosingImageTemp_erode,ClosingImageTemp_dilate,NULL,1);
	cvMorphologyEx(ClosingImageTemp_dilate,ClosingImage,ClosingImageTemp,NULL,CV_MOP_CLOSE,4);
	cvErode(ClosingImage,ClosingImageTemp_erode,NULL,2);
	cvDilate(ClosingImageTemp_erode,ClosingImageTemp_dilate,NULL,2);
	QueryPerformanceCounter(&endTime);
	ClosingTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+ClosingTime;
#pragma endregion Closing
#pragma region (Input:ClosingImageTemp_dilate,Output:LabelingImage)
	// Labeling
	QueryPerformanceCounter(&startTime);
	IplImage *LabelingImage = cvCreateImage(cvGetSize(ClosingImageTemp_dilate),IPL_DEPTH_8U,3);
	CvMemStorage  *mem = cvCreateMemStorage(0);
	CvSeq *contours = 0;
	cvFindContours(ClosingImageTemp_dilate,mem,&contours,sizeof(CvContour),CV_RETR_CCOMP,CV_CHAIN_APPROX_NONE,cvPoint(0,0));
	unsigned long long c=0;
	for (; contours != 0; contours = contours->h_next)
	{		
		c+=LabelColorOrder;
		if (c>MaxLabelingCounter){
			printf("Labeling counter be overflow. \n");
			break;
		}
		CvScalar ext_color = CV_RGB( (c%255)&255, ((c>>8)%255)&255, (c>>16)&255 ); //randomly coloring different contours
		cvDrawContours(LabelingImage, contours, ext_color, CV_RGB(0,0,0), -1, CV_FILLED, 8, cvPoint(0,0));
	}
	QueryPerformanceCounter(&endTime);
	LabelingTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+LabelingTime;
#pragma endregion Labeling
#pragma region (Input:LabelingImage,Output:LabelingImage_gray)
	// Select Region
	QueryPerformanceCounter(&startTime);
	IplImage *LabelingImage_gray = cvCreateImage(cvGetSize(LabelingImage),IPL_DEPTH_8U,1);
	BDThres BDThreshold;
	BDThreshold.BrightThreshold=BrightThresholdValue;
	BDThreshold.DarkThreshold=DarkThresholdValue;
	pixelBGR *LabelOBJhead=(pixelBGR*)calloc(1,sizeof(pixelBGR));
	for (int y=0;y<LabelingImage->height;y++){
		for (int x=0;x<LabelingImage->widthStep;x+=3){
			if ( (LabelingImage->imageData[y*LabelingImage->widthStep+x]!=0) ||	
				(LabelingImage->imageData[y*LabelingImage->widthStep+x+1]!=0) || 
				(LabelingImage->imageData[y*LabelingImage->widthStep+x+2]!=0) ){
					CvScalar OBJcolor={LabelingImage->imageData[y*LabelingImage->widthStep+x],
						LabelingImage->imageData[y*LabelingImage->widthStep+x+1],
						LabelingImage->imageData[y*LabelingImage->widthStep+x+2],0};
					addLabelObj(LabelOBJhead,(unsigned char)inputImage->imageData[y*inputImage->widthStep+x/3],	BDThreshold,OBJcolor);
			}
		}
	}	/*
	int count=0;
	pixelBGR *tempf=LabelOBJhead;
	while(tempf!=NULL){
		count++;
		//printf("Label%d.TC=%d\n",count,tempf->TotalCount);
		//printf("Label%d.BT=%d\n",count,tempf->BrightThreshold);
		//printf("Label%d.BC=%d\n",count,tempf->BrightCount);
		//printf("Label%d.DT=%d\n",count,tempf->DarkThreshold);
		//printf("Label%d.DC=%d\n",count,tempf->DarkCount);
		//printf("\n");
		tempf=tempf->nextOBJ;
	}*/
	for (int y=0;y<LabelingImage->height;y++){
		for (int x=0;x<LabelingImage->widthStep;x+=3){
			if ( (LabelingImage->imageData[y*LabelingImage->widthStep+x]!=0) ||	
				(LabelingImage->imageData[y*LabelingImage->widthStep+x+1]!=0) || 
				(LabelingImage->imageData[y*LabelingImage->widthStep+x+2]!=0) ){
					CvScalar OBJcolor={LabelingImage->imageData[y*LabelingImage->widthStep+x],
						LabelingImage->imageData[y*LabelingImage->widthStep+x+1],
						LabelingImage->imageData[y*LabelingImage->widthStep+x+2],0};
					pixelBGR *temp=searchObj(LabelOBJhead,OBJcolor);
					if (temp->BrightCount>=BrightCountF)
						LabelingImage_gray->imageData[y*LabelingImage_gray->widthStep+x/3]=0;
					else if (temp->DarkCount>=DarkCountF)
						LabelingImage_gray->imageData[y*LabelingImage_gray->widthStep+x/3]=0;
					else if (temp->TotalCount<=TotalCountF)
						LabelingImage_gray->imageData[y*LabelingImage_gray->widthStep+x/3]=0;
					else
						LabelingImage_gray->imageData[y*LabelingImage_gray->widthStep+x/3]=255;
			}
		}
	}
	QueryPerformanceCounter(&endTime);
	SelectLabelingTime=1000*((double)endTime.QuadPart-(double)startTime.QuadPart)/freq.QuadPart;
	TotalTime=TotalTime+SelectLabelingTime;
#pragma endregion Select Region

	cvSaveImage("D:/Workspace/Workspace_VC/GolfBallDefect/GolfBallDefect/Debug/LabelingImage.bmp",LabelingImage);
	cvSaveImage("D:/Workspace/Workspace_VC/GolfBallDefect/GolfBallDefect/Debug/LabelingImage_gray.bmp",LabelingImage_gray);
	
#pragma region Message Show
	// Show text message
	printf("Load Image Time = %f (ms)\n",loadTime);
	printf("Cut ROI Time = %f (ms)\n",ROITime);
	printf("Preprocess Time = %f (ms)\n",preprocessTime);
	printf("Sobel Process Time = %f (ms)\n",SobelTime);
	printf("Closing Process Time = %f (ms)\n",ClosingTime);
	printf("Labeling Process Time = %f (ms)\n",LabelingTime);
	printf("Select Labeling Time = %f (ms)\n",SelectLabelingTime);
	printf("///////////////Result///////////////\n");
	printf("Total Process Time = %f (ms)\n",TotalTime);
#pragma endregion Message Show
#pragma region Image Show
	// Show Image
	cvShowImage("Original Image", inputImage); 
	cvShowImage("Preprocess Image", preprocessImage); 
	cvShowImage("Sobel Image",SobelImage);
	cvShowImage("Closing Image",ClosingImage);
	cvShowImage("Labeling Image",LabelingImage);
	cvShowImage("Labeling_gary Image",LabelingImage_gray);
#pragma endregion Image Show

#pragma region Dispose
	// // Dispose
	// LinkList
	freeLabelObj(LabelOBJhead);
	// Image
	cvReleaseImage(&inputImage);
	cvReleaseImage(&inputImage_RGB);
	//cvReleaseImage(&preprocessImage);
	cvReleaseImage(&SobelImage_8U);
	cvReleaseImage(&SobelImage_16S);
	cvReleaseImage(&SobelImage);
	cvReleaseImage(&ClosingImage);
	cvReleaseImage(&ClosingImageTemp);
	cvReleaseImage(&ClosingImageTemp_erode);
	cvReleaseImage(&ClosingImageTemp_dilate);
	cvReleaseImage(&LabelingImage);
	cvReleaseImage(&LabelingImage_gray);
#pragma endregion Dispose

	// waiting key in to finish this program
    cv::waitKey(0);

}


