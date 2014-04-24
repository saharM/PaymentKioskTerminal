#include "stdafx.h"
#include "COM.H"

#include "stdlib.h"
Tcomm::Tcomm(void)
{
    ZeroMemory((char *)&dcbs, sizeof(dcbs));
    ZeroMemory((char *)&lpctmo, sizeof(lpctmo));
    hCom=NULL;
}
Tcomm::~Tcomm(void)
{
    if(hCom!=NULL)   CloseHandle(hCom);
    hCom=NULL;
}
int Tcomm::Open_Com(char * ComPort,long Baud)
{
#ifndef BRIDGE
    char sm[6] = "", temp[10] = "";
    if (hCom != NULL) CloseHandle( hCom );
    strncpy(sm, ComPort, 5); sm[5] = 0;
    strcpy(t_buf, sm); t_buf[4] = 0;// t_buf
    hCom=CreateFile(ComPort, GENERIC_READ | GENERIC_WRITE, NULL, 0, OPEN_EXISTING, FILE_ATTRIBUTE_ARCHIVE, (HANDLE)NULL);
    if (hCom == NULL) return -1;
    if (! SetupComm( hCom, MAX_RWBUF, MAX_RWBUF )) return -2;
    PurgeComm( hCom, PURGE_TXCLEAR | PURGE_RXCLEAR );
    lpctmo.ReadTotalTimeoutMultiplier = 6;
    strcpy(t_buf, sm);
    ltoa(Baud, temp, 10);
    strcat(t_buf, " baud=");
    strcat(t_buf, temp);
    strcat(t_buf, " parity=N data=8 stop=1");
    return  Set_Com();
#else
    char szbuz[128] = "";
    GetPrivateProfileString("Config", "Remote ip", "", ip_remote, sizeof(ip_remote), ".\\bridge.ini");
    if( ! strlen( ip_remote ))
    {
    WritePrivateProfileString("Config", "Remote ip", "192.168.0.136", ".\\bridge.ini");
    WritePrivateProfileString("Config", "Remote Port", "3000", ".\\bridge.ini");
    WritePrivateProfileString("Config", "Local ip", "", ".\\bridge.ini");
    WritePrivateProfileString("Config", "Local Port", "3000", ".\\bridge.ini");
    WritePrivateProfileString("Config", "Time Out", "3000", ".\\bridge.ini");
    }

    GetPrivateProfileString("Config", "Time Out", "1000", szbuz, sizeof(szbuz), ".\\bridge.ini");
    timeout = atoi(szbuz);
    
    ZeroMemory(szbuz, sizeof(szbuz));
    GetPrivateProfileString("Config", "Local Port", "3000", szbuz, sizeof(szbuz), ".\\bridge.ini");
    port_local = atoi(szbuz);

    ZeroMemory(szbuz, sizeof(szbuz));
    GetPrivateProfileString("Config", "Remote Port", "3000", szbuz, sizeof(szbuz), ".\\bridge.ini");
    port_remote = atoi(szbuz);

    GetPrivateProfileString("Config", "Local ip", "", ip_local, sizeof(ip_local), ".\\bridge.ini");

    udp_init();
    Set_Timeout(timeout);

    udp_bind(ip_local, port_local);
    return 0;
#endif    
}
int Tcomm::Set_Com(void)
{
#ifndef BRIDGE
    if ( ! BuildCommDCB(t_buf, &dcbs))     return -3;
    dcbs.fDtrControl =1;
    if ( ! SetCommState(hCom, &dcbs))      return -4;
    if ( ! SetCommTimeouts(hCom, &lpctmo)) return -5;
#endif    
    return 0;
}
int Tcomm::Close_Com(void)
{
#ifndef BRIDGE
    if (hCom == NULL) return 0;
    CloseHandle( hCom );
    hCom = NULL;
#else
    ZeroMemory(ip_remote, sizeof(ip_remote));
    ZeroMemory(ip_local, sizeof(ip_local));
    udp_close();    
#endif    
    return 0;

}
int Tcomm::Write_Comm(unsigned char * MSG,int Leng)
{
    int i;
#ifndef BRIDGE
    WriteFile(hCom, (LPCVOID)MSG, (DWORD)Leng, (LPDWORD)(&i), NULL);
#else
    udp_send(MSG, Leng, ip_remote, port_remote); 
#endif
    return(i);
}
int Tcomm::Read_Comm(unsigned char * MSG,int Leng)
{
    int i;
#ifndef BRIDGE
    ReadFile( hCom, (LPVOID)MSG, (DWORD)Leng, (LPDWORD)(&i), NULL);
#else
    char ip[32] = "";
    DWORD port;
    i = udp_recv(MSG, Leng, ip, port);
#endif
    return (i);
}
int  Tcomm::SetTimeOut(int val)
{
#ifndef BRIDGE
    if(hCom == NULL) return -1;
    lpctmo.ReadTotalTimeoutMultiplier = val;
    return SetCommTimeouts(hCom, &lpctmo);
#else
    return 0;
#endif
}
int  Tcomm::ClearTxRx(void)
{
#ifndef BRIDGE
    if(hCom==NULL) return -1;
    return PurgeComm(hCom, PURGE_TXCLEAR | PURGE_RXCLEAR);
#else
    DWORD ln;
    int dwSenderSize = sizeof(sck_add);
    char *szbuz;
    if(ln = Amount())
    {
    szbuz = new char[ln + 1];
    ZeroMemory(szbuz, ln + 1);
    recvfrom(sck, szbuz, ln, 0,
          (SOCKADDR *)&sck_add, &dwSenderSize);
    delete[] szbuz;
    }
    return 0;
#endif
}
