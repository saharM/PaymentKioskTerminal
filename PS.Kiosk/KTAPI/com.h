#include "windows.h"
#define MAX_RWBUF 512
//#define BRIDGE
#ifdef BRIDGE
#include "net\net.h"
class Tcomm : public TNet
#else
class Tcomm
#endif
{
private:
COMMTIMEOUTS lpctmo;
DCB          dcbs;
int 	     nComID;
char 	     message[48];
char	     t_buf[100];
int Set_Com(void);

#ifdef BRIDGE
char ip_local[32],
     ip_remote[32];
DWORD port_local,
      port_remote,
      timeout;  
#endif

public:
//char *RW_BUF;
int  result;
int  lp;
HANDLE	     hCom;
 Tcomm(void);
~Tcomm(void);

int  Open_Com(char * ComPort,long Baud);
int  Close_Com(void);
int  Write_Comm(unsigned char * MSG,int Leng);
int  Read_Comm(unsigned char * MSG,int Leng) ;

int  SetTimeOut(int val);
int  ClearTxRx(void);
}  ;
 