// Keyu.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "com.h"
#include "des.h"
#include <iostream>
#include <fstream>
using namespace std;

Tcomm tcom;
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    return TRUE;
}
//µÈ´ý·µ»ØÊý¾Ý°ü
//»¹»Ø----ST
//str-----·µ»ØµÄÊý¾Ý´®
//·µ»ØÎªÊµ¼ÊÊý¾ÝµÄ³¤¶È
void LogFile(char * msg)
{
	ofstream myfile; 
    myfile.open ("C:\\EppLog.txt");
    myfile << msg;
    myfile.close();

}

int WINAPI Pinpad_Return_All(unsigned char *str,int L)
{
	unsigned char temp_return[10]="";
	unsigned char str1[500]="";
	int i, j, k;
	int ST=0x80;
	k=0;
	i=0;
	for(j=0;j<500;j++)
	{
		if(k >= L)
			break;
		i=tcom.Read_Comm(temp_return,1);
		if(i>0)
		{			
			str1[k] = temp_return[0];
			k++;
		}
	}
	if(str1[0] == 0x02 && str1[1] == 0x02 && str1[2] == 0x4F && str1[3] == 0x4B)
	{
		ST = 0x00;
	}
	else if (str1[0] == 0x02 && str1[1] == 0x02 && str1[2] == 0x45 && str1[3] == 0x52)
	{
		ST = 0x01;
	}
	else
	{
		if (str1[0] == 0x02)
		{
			for (int o = 0; o < str1[1]; o++){
				str[o] = str1[2+o];
			}
			ST = 0x00;
		}
	}		
	return ST;
}
//×Ö·û´®±äÎª16½øÖÆÊý×é
/*
int WINAPI Str_Array_Signal(char *str1,unsigned char *str2)//str1Îª×Ö·û´®£¬STR2ÎªÊý×é
{
	unsigned int i;
	unsigned int length;
	length=strlen(str1);
    for(i=0;i<length;i++)
	{
		str2[i]=str1[i];
		}

 return i;
}

//16½øÖÆÊý×é±äÎª×Ö·û´®
int WINAPI Array_String(unsigned char *str1,char *str2)//str1Îª×Ö·û´®£¬STR2ÎªÊý×é
{
	unsigned int i;
	unsigned int length=10;
    for(i=0;i<length;i++)
	{
		str2[i]=str1[i];
		}
    return i;
}

//×Ö·û´®±äÎª16½øÖÆÊý×é
int WINAPI Str_Array(char *str1,unsigned char *str2)//str1Îª×Ö·û´®£¬STR2ÎªÊý×é
{
	int i;
	unsigned char H_BYTE;
	unsigned char L_BYTE;
	int length;
	int k;
	int j;
	char str[500]="";;
	length=strlen(str1);
	k=0;
	for(i=0;i<length;i++)
	{
		if(str1[i]!=' ')
		{
			str[k]=str1[i];
			k++;
		}
	}
    length=strlen(str);
	length=length/2;
	k=0;	
	j=0;
	for(i=0;i<length;i++)
	{
		
		H_BYTE=str[j];
		j++;
		L_BYTE=str[j];
		j++;
		
		if((H_BYTE>=0x30) && (H_BYTE<=0x39))
		 {
			H_BYTE=(H_BYTE<<4) & 0xf0;;
		 }
		 else if((H_BYTE>='A') && (H_BYTE <='F'))
		 {
			H_BYTE=((H_BYTE-0x37)<<4) & 0xf0 ;
		 }
		 else if((H_BYTE>='a') && (H_BYTE<='f'))
		 {
			H_BYTE=((H_BYTE-0x57)<<4) & 0xf0 ;
		 }
		else 
			H_BYTE=0;

		if((L_BYTE>=0x30) && (L_BYTE<=0x39))
		{
			L_BYTE=L_BYTE & 0x0F;;
		}
		else if((L_BYTE>='A') && (L_BYTE <='F'))
		{
			L_BYTE=(L_BYTE-0x37) & 0x0F ;
		}
		else if((L_BYTE>='a') && (L_BYTE<='f'))
		{
			L_BYTE=(L_BYTE-0x57) & 0x0F ;
		 }
		 else 
			L_BYTE=0;
		 str2[k]=H_BYTE+L_BYTE;
		 k++;
	   }
 return k;
}
*/
//²ð·ÖµÄ½á¹û£¬STR2[0]ÊÇ¿ÕµÄ,
//·µ»ØÊý×éµÄ³¤¶È
//STR2[0]ÊÇ¿ÕµÄ£¬ÓÃÓÚ·µ»Ø02±í±êÊ¾Í¼
unsigned int WINAPI Byte_One_Two(unsigned char *str1,unsigned char *str2,unsigned int length)//Êý×é½øÐÐ37 ²ð·Ö
{
 unsigned char x,y;
 unsigned int i,k;
 k=1;
 str2[0]=0x02;
 for(i=0;i<length;i++)
 {
	 x=(str1[i]>>4);
	 y=(str1[i] & 0x0f);
	 if(x>=0x0a)
		 x=x+7;
	 if(y>=0x0a)
		 y=y+7;
     x=x+0x30;
	 y=y+0x30;
	 str2[k++]=x;
	 str2[k++]=y;
}
 str2[k++]=0x03;//¼ÓÉÏ½áÊø±êÊ¾
 return k;
}
//Êý×é½øÐÐ37 Ñ¹Ëõ
int WINAPI Byte_Two_One(unsigned char *str1,unsigned char *str2,unsigned int length)
{
 unsigned char H_BYTE,L_BYTE;
 unsigned int i,k;
 k=0;
 length=length/2;
 for(i=0;i<length;i++)
  	{
  	    H_BYTE=str1[k]-0x30;
		k++;
  	    L_BYTE=str1[k]-0x30;
		k++;
  	    if(H_BYTE>=0x0A)//37·Ö
  	    H_BYTE=H_BYTE-7;
  	    if(L_BYTE>=0x0A)
  	    L_BYTE=L_BYTE-7;
  	          
  	    str2[i]=(H_BYTE<<4)+L_BYTE;
  	       
  	 }
 return 1;
}
//´ò¿ª´®¿Ú
int WINAPI PinPad_EnablePort(int n)
{
	int i;
	if(n==1)
    i=tcom.Open_Com("COM1",9600);
    else if(n==2)
    i=tcom.Open_Com("COM2",9600);
    else if(n==3)
    i=tcom.Open_Com("COM3",9600);
	else if(n==4)
    i=tcom.Open_Com("COM4",9600);
	else if(n==5)
    i=tcom.Open_Com("COM5",9600);
    else if(n==6)
    i=tcom.Open_Com("COM6",9600);
	else if(n==7)
    i=tcom.Open_Com("COM7",9600);
	else if(n==8)
    i=tcom.Open_Com("COM8",9600);
    else if(n==9)
    i=tcom.Open_Com("COM9",9600);
	else if(n==10)
    i=tcom.Open_Com("COM10",9600);
	return i;
    /*if(i==0)
		return 0x14;
	else
		return 0x15;*/
	
 }
//¹Ø±Õ´®¿Ú
int WINAPI PinPad_DisablePort(void)
{
	return tcom.Close_Com();
 }
//È¡²úÆ·ÖÕ¶ËºÅ
int WINAPI PinPad_ReadEndNo(unsigned char *str)
{//02 01 38 03
	int i;
	int ST;
	unsigned char str1[6]={0x02,0x30,0x31,0x33,0x38,0x03};
	i=tcom.Write_Comm(str1,6);
	if(i==6)
	{
        ST=Pinpad_Return_All(str,8);
		return ST;
	}
	else 
		return 0;
 }
//ÉèÖÃ²úÆ·ÖÕ¶ËºÅ
//02h+09h+<ST>+<Terminal character string >+03h
int WINAPI PinPad_WriteEndNo(unsigned char *str)
{
	int i;
	int ST;
	int length;
	unsigned char str1[15]="";
	unsigned char str2[22]="";
	str1[0]=0x09;
	str1[1]=0x38;
	for(i=0;i<8;i++)
	str1[2+i]=str[i];
    length=Byte_One_Two(str1,str2,i+2);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str2,length);
	if(i==length)
	{
        ST=Pinpad_Return_All(str,0);
		return ST;
	}
	else 
		return 0;
 }

//¸´Î»ÃÜÂë¼üÅÌ,¸Ä±äÃÜÔ¿
//1Bh+52h+0Dh+0Ah
int WINAPI PinPad_InitKey(void)
{//02h+01h+31h+03h
	int i;
	int ST;
	unsigned char str[10]="";
	unsigned char str1[6]={0x1B,0x52,0x0D,0x0A};
	i=tcom.Write_Comm(str1,4);
	if(i==4)
	{
	    ST=Pinpad_Return_All(str,4);
		return ST;
	}
	else 
		return 0;
 }
//ÏÂÔÖÖ÷ÃÜÔ¿
//DATA1-----Ö÷ÃÜÔ¿ºÅ
//DATA2-----¼ÓÃÜÏÂÔØ1/Ã÷ÂëÏÂÔØ2
//*DATA3----ÃÜÔ¿×Ö·û´®
//02h+12h+32h+<M>+<MK1>+<MK2>+03h   
//02h+22h+32h+<M>+<MK1>+<MK2>+03h  
int WINAPI PinPad_DownloadMKey(unsigned char M,unsigned char *MK1,unsigned char *MK2,unsigned char Length)
{
	unsigned char str[40]="";
	//unsigned char str1[80]="";
	int i;
	//int length;
	unsigned char ST;
	/*if(Length==8)
	str[0]=0x12;
	else if(Length==16)*/
	str[0]=0x1B;
	str[1]=0x4D;
	str[2]=M;
	str[3]=Length;
	for(i=0;i<Length;i++)
		str[4+i]=MK1[i];
	for(i=0;i<Length;i++)
		str[4+Length+i]=MK2[i];
	str[4+(2*Length)]=0x0D;
	str[5+(2*Length)]=0x0A;
     //length=Byte_One_Two(str,str1,Length+Length+3);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str,6 + (2 * Length));
	if(i==6 + (2 * Length))
	 {
        ST=Pinpad_Return_All(str,4);
		return ST;
	}
	 else 
		return 0;
 }
//ÏÂÔØ¹¤×÷ÃÜÔ¿,ÃÜÎÄÏÂÔØ
//DATA1-----Ö÷ÃÜÔ¿ºÅ
//DATA2-----¹¤×÷ÃÜÔ¿ºÅ
//*DATA3----ÃÜÔ¿×Ö·û´®
//02h+0Bh+33h+<M>+<N>+<WP>+03h or 
//02h+13h+33h+<M>+<N>+<WP>+03h
int WINAPI PinPad_DownloadEncryWKey(unsigned char M,unsigned char N,unsigned char *WP,unsigned char Length)//ÏÂÔØ¹¤×÷ÃÜÔ¿
{
	unsigned char str[30]="";
	//unsigned char str1[50]="";
	int i;
	//int length;
	unsigned char ST;
    /*if(Length==8)
		str[0]=0x0B;
	else if(Length==16)
		str[0]=0x13;*/
	str[0]=0x1B;
	str[1]=0x4B;
	str[2]=M;
	str[3]=N;
	str[4]=Length;
	for(i=0;i<Length;i++)
		str[5+i]=WP[i];
     //length=Byte_One_Two(str,str1,i+4);//²ð·Ö£¬½á¹ûALL2
	str[5+Length]=0x0D;
	str[6+Length]=0x0A;
	i=tcom.Write_Comm(str,7+Length);
	if(i==7+Length)
	{ 
		ST=Pinpad_Return_All(str,4);
		return ST;
	}
	else 
		return 0;

 }
//ÏÂÔØ¹¤×÷ÃÜÔ¿,Ã÷ÎÄÏÂÔØ
//M-----Ö÷ÃÜÔ¿ºÅ
//N-----¹¤×÷ÃÜÔ¿ºÅ
//*DATA3----ÃÜÔ¿×Ö·û´®
//02h+13h+34h+<M>+<N>+<WK1>+<WK2>+03h or 
//02h+23h+34h+<M>+<N>+<WK1>+<WK2>+03h
//0Bh+55h+<M>+<N>+<Length>+<WK1>+<WK2>+03h
int WINAPI PinPad_DownloadEncryplainWKey(unsigned char M,unsigned char N,unsigned char *WK1,unsigned char *WK2,unsigned char Length)//ÏÂÔØ¹¤×÷ÃÜÔ¿
{
	unsigned char str[50]="";
	//unsigned char str1[80]="";
	int i;
	//int length;
	unsigned char ST;
    /*if(Length==8)
		str[0]=0x13;
	else if(Length==16)
		str[0]=0x23;*/
	str[0]=0x1B;
	str[1]=0x55;
	str[2]=M;
	str[3]=N;
	str[4]=Length;
	for(i=0;i<Length;i++)
		str[5+i]=WK1[i];
	for(i=0;i<Length;i++)
		str[5+i+Length]=WK2[i];
	str[5+(2*Length)]=0x0D;
	str[6+(2*Length)]=0x0A;
    /*length=Byte_One_Two(str,str1,Length+Length+4);//²ð·Ö£¬½á¹ûALL2*/
	i=tcom.Write_Comm(str,7+(2*Length));
	if(i==7+(2*Length))
	{ 
		ST=Pinpad_Return_All(str,4);
		return ST;
	}
	else 
		return 0;

 }
//¼¤»î¹¤×÷ÃÜÔ¿
//M-----Ö÷ÃÜÔ¿ºÅ
//N-----¹¤×÷ÃÜÔ¿ºÅ
//02h+03h+35h+<M>+<N>+03h
//1Bh+41h+<M>+<N>+0Dh+0Ah
int WINAPI PinPad_ActiveWKey(unsigned char M,unsigned char N)
{
	unsigned char str[6]="";
	//unsigned char str1[11]="";
	//unsigned char length;
	int ST;
	short i;
   	str[0]=0x1B;
	str[1]=0x41;
	str[2]=M;
	str[3]=N;
	str[4]=0x0D;
	str[5]=0x0A;
    //length=Byte_One_Two(str,str1,4);//²ð·Ö£¬½á¹ûALL2
   	i=tcom.Write_Comm(str,6);
	if(i==6)
	{
       ST=Pinpad_Return_All(str,4);
		return ST;
	}
	else 
		return 0;
 }
//
//
//
//
//
//1Bh+30h+0Dh+0Ah
int WINAPI PinPad_ExitEPP(void)
{
	unsigned char str[5]="";
	unsigned char ST;
	unsigned char i;
   	str[0]=0x1B;
	str[1]=0x30;
	str[2]=0x0D;
	str[3]=0x0A;
    //length=Byte_One_Two(str,str1,5);//²ð·Ö£¬½á¹ûALL2
    i=tcom.Write_Comm(str,4);
	//if(i==4)
	//{
    //    ST=Pinpad_Return_All(str,0);
	//	return ST;
	//}
	//else 
		return 0;
 }
//1Bh+58h+<PIN-L>+<JMD>+0Dh+0Ah
int WINAPI PinPad_StartEPP(unsigned char PIN_L,unsigned char JMD)
{
	try
	{

		LogFile("Enter to PinPad_StartEPP");

		unsigned char str[10]="";
			//unsigned char length;
	//unsigned char str1[12]="";
		unsigned char ST;
		unsigned char i;
   		str[0]=0x1B;
		str[1]=0x58;
		str[2]=PIN_L;
		str[3]=JMD;
		str[4]=0x0D;
		str[5]=0x0A;
		//length=Byte_One_Two(str,str1,5);//²ð·Ö£¬½á¹ûALL2
		i=tcom.Write_Comm(str,6);
		if(i==6)
		{
			ST=Pinpad_Return_All(str,4);
			return ST;
		}
		else 
			return 0;
	}
	catch(int k)
	{
		/*std::string str;
		str = "Error in  PinPad_StartEPP , ex =";
		str.append(ex)*/
		LogFile("Error in  PinPad_StartEPP ");
		return -1;
	}
 }

//·¢ËÍ¿ª¹Ø¼üÅÌ¡¢°´¼üÉùÒô
//DATA1-----²ÎÊý
//02h+02h+45h+<CTL>+03h
int WINAPI PinPad_SetBeep(unsigned char x)
{
	try
	{
	unsigned char str[5]="";
	unsigned char str1[100]="";
	unsigned char length;
	int ST;
	int i;
   	str[0]=0x1B; //0x02;
	str[1]=0x43;
	str[2]=0x32;
	str[3]=0x0D;
	str[4]=0x0A;
	//length=Byte_One_Two(str,str1,5);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str,5);//length);
	if(i==5)
	{
        ST=Pinpad_Return_All(str,4);
     	return ST;
	}
	else 
		return 0;
	}
	catch(char ex)
	{
		//LogFile(ex);
		return -1;
	}

 }
//Éè¶¨Ëã·¨´¦Àí²ÎÊý
//DATA1-----P
//DATA2-----F
//02h+03h+36h+<P>+<F>+03h
int WINAPI PinPad_SetParameters(unsigned char x)
{
	try
	{
		LogFile("PinPad_SetParameters");
	unsigned char str[5];
	//unsigned char str1[10];
	//unsigned char length;
	int i;
	int ST;
   	str[0]=0x1B;
	str[1]=0x50;
	str[2]=x;
	str[3]=0x0D;
	str[4]=0x0A;
	
    //length=Byte_One_Two(str,str1,4);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str,5);
	if(i==5)
	{
        ST=Pinpad_Return_All(str,4);
		return ST;
	}
	else 
		return 0;
	}
	catch(char ex)
	{
		
		return -1;
	}

 }
//´Ó´®¿Ú½ÓÊÕÒ»¸ö×Ö·û
//DATA1-----½ÓÊÕµÄ×Ö·û
int WINAPI PinPad_ReadOneByte(unsigned char *str)
{
	try
	{
		LogFile("Enter in PinPad_ReadOneByte");
    int i;  
    i=tcom.Read_Comm(str,1) ;
	if(i==1)
		return 1;
	else 
		return 0;
	}
	catch(char ex)
	{
		LogFile("Exception in PinPad_ReadOneByte");
		return -1;
		
	}

 }
//Êý¾ÝMAC¼ÆËã
//str1-----ÐèÒª¼ÆËãµÄMACÊý¾Ý
//str2-----MACÖµ8×Ö½Ú³¤¶È
//Length---str1µÄ³¤¶È
//02h+<Ln>+43h+<character string>+03h 
//1Bh+61h+<Ln>+<character string>+0Dh+0Ah
int WINAPI PinPad_MakeMac(unsigned char *str1,unsigned char *str2,unsigned char Length)
{
	try
	{
		LogFile("Enter in PinPad_MakeMac");
	unsigned char str3[256]="";
	//int length;
	//unsigned char str4[512]="";
	int i;
   	int ST;
	str3[0]=0x1B;
	str3[1]=0x61;
	str3[2]=Length;
	for(i=0;i<Length;i++)
		str3[3+i]=str1[i];
	str3[3+Length]=0x0D;
	str3[4+Length]=0x0A;
    //length=Byte_One_Two(str3,str4,Length+2);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str3,5+Length);
	if(5+Length==i)
	{
       ST=Pinpad_Return_All(str2,10);
		return ST;
	}
	else 
		return 0;
	}
	catch(char ex)
	{
		LogFile("Exception in PinPad_MakeMac");
	}

 }
//²âÊÔ¼üÅÌÏìÓ¦×Ö·û
//DATA1-----ÃÜÂë
//02h+02h+44h+<CHR>+03h
int WINAPI PinPad_RChar(unsigned char *str)
{
	unsigned char str1[3]="";
	unsigned char length;
	int i;
	int ST;
	unsigned char str2[8];
	
 	str1[0]=0x02;
	str1[1]=0x44;
	str1[2]=str[0];
	
    length=Byte_One_Two(str1,str2,3);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str2,length);
	if(length==i)
	{
       ST=Pinpad_Return_All(str,1);
		return ST;
	}
	else 
		return 0;

 }
//È¡¼üÅÌÖÐÃÜÂë
//DATA1-----ÃÜÂë
//02h+01h+40h+03h
//1Bh+47h+0Dh+0Ah
int WINAPI PinPad_ReadPin(unsigned char *str)
{
	try
	{
		LogFile("Enter in PinPad_ReadPin");
	unsigned char str1[4]={0x1B,0x47,0x0D,0x0A};
	unsigned int i;
	int ST;
 	i=tcom.Write_Comm(str1,4);
	if(4==i)
	{
        ST=Pinpad_Return_All(str,10);
		return ST;
	}
	else 
		return 0;
	}
	catch(char ex)
	{
		LogFile("Exception in PinPad_ReadPin");
	}
 }
//ÏÂÔØ¿¨ºÅ»òÕßÕÊºÅ
//DATA1-----¿¨ºÅ
//02h+0Dh+37h+<CARD-NO>+03h or 
//02h+0Bh+37h+<TRANS-Code>+03h
//1Bh+60h+0Ch+<CARD-NO>+0Dh +0Ah or
//1Bh+60h+0Ah+<TRANS-Code>+0Dh+0Ah
int WINAPI PinPad_DownloadCNo(unsigned char *str,int Length)
{
	unsigned char str1[30]="";
	//unsigned char str2[30]="";
	//unsigned char length;
	int i;
	int ST;
 	str1[0]=0x1B;
	str1[1]=0x60;
	str1[2]=0x0C;	
	for(i=0;i<Length;i++)
		str1[3+i]=str[i];
	str1[3+Length]=0x0D;
	str1[4+Length]=0x0A;
    //length=Byte_One_Two(str1,str2, Length+2);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str1,5+Length);
	if(5+Length==i)
	{
        ST=Pinpad_Return_All(str1,4);
		return ST;
	}
	else 
		return 0;
 }
//Êý¾Ý¼ÓÃÜ
//DATA1-----ÐèÒª¼ÓÃÜµÄÊý¾Ý
//DATA2-----ÐèÒª¼ÓÃÜµÄÊý¾Ý³¤¶È
//DATA3-----¼ÓÃÜºóµÄÊý¾Ý
//02h+Lnh+41h+<plain text character string>+03h
//1Bh+48h+31h+Ln+<plain text character string>+0Dh+0Ah
int WINAPI PinPad_EncryData(unsigned char *str1,unsigned char *str2,int Length)
{
	unsigned char str3[512]="";
	//unsigned char str4[256]="";
	//unsigned char length;
	int i;
    int ST;
 	str3[0]=0x1B;
	str3[1]=0x48;
	str3[2]=0x31;
	str3[3]=Length;
    for(i=0;i<Length;i++)
		str3[i+4]=str1[i];
	str3[4+Length]=0x0D;
	str3[5+Length]=0x0A;
	//length=Byte_One_Two(str4,str3, Length+2);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str3,6+Length);
	if(6+Length==i)
	{
        ST=Pinpad_Return_All(str2,2 + Length);
		return ST;
	}
	else 
		return 0;
 }
//Êý¾Ý½âÃÜ
//DATA1-----ÐèÒª½âÃÜµÄÊý¾Ý
//DATA2-----ÐèÒª½âÃÜµÄÊý¾Ý³¤¶È
//DATA3-----½âÃÜºóµÄÊý¾Ý
//02h+Lnh+42h+<Encryption text character string>+03h
//1Bh+48h+32h+Ln+<Encryption text character string>+0Dh+0Ah
int WINAPI PinPad_DecryData(unsigned char *str1,unsigned char *str2,int Length)
{
	unsigned char str3[512]="";
	//unsigned char str4[256]="";
	//unsigned char length;
	int i;
    int ST;
 	str3[0]=0x1B;
	str3[1]=0x48;
	str3[2]=0x32;
	str3[3]=Length;
    for(i=0;i<Length;i++)
		str3[i+4]=str1[i];
	str3[4+Length]=0x0D;
	str3[5+Length]=0x0A;
	//length=Byte_One_Two(str4,str3, Length+2);//²ð·Ö£¬½á¹ûALL2
	i=tcom.Write_Comm(str3,6+Length);
	if(6+Length==i)
	{
        ST=Pinpad_Return_All(str2,2 + Length);
		return ST;
	}
	else 
		return 0;
}
//DES¹¦ÄÜº¯Êýµ÷ÓÃ
//surrce-----ÐèÒª½âÃÜµÄÊý¾Ý
//dest-----¼Ó½âÃÜºóµÄÊý¾Ý
//inkey-----ÃÜÔ¿
//flg----¼Ó/½âÃÜ
//TDES_DES---DES×´Ì¬
int WINAPI TDES_FUN(unsigned char *source, unsigned char *dest, unsigned char *inkey,unsigned char flg,unsigned char TDES_DES)
{
      unsigned char inkey_temp[8];
      unsigned char i;
      
      for(i=0;i<8;i++)
      inkey_temp[i]=inkey[i];
      Des_Fun(source, dest, inkey_temp, flg);
      
      if((TDES_DES==2) || (TDES_DES==3)) //3des_3key\\3des_2key
      {
      	if(flg==1)
      	flg=0;
      	else 
      	flg=1;
      
      	for(i=0;i<8;i++)
      	inkey_temp[i]=inkey[i+8];
      	Des_Fun(dest, source, inkey_temp, flg);
      	
      	if(flg==1)
      	flg=0;
      	else 
      	flg=1;
      	
        if(TDES_DES==3)
      	for(i=0;i<8;i++)
      	inkey_temp[i]=inkey[i+16];
      	else
      	for(i=0;i<8;i++)
      	inkey_temp[i]=inkey[i];
      	
      	Des_Fun(source, dest, inkey_temp, flg);
     }
	  return 1;
}


