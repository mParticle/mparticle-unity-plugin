#ifndef mParticle_mParticleUnity_h
#define mParticle_mParticleUnity_h

void _Initialize(const char *optionsJSON);
int _GetEnvironment(void *empty);
void _SetOptOut(int optOut);
void _SetUploadInterval(int uploadInterval);

void _LogEvent(const char *mpEvent);
void _LogCommerceEvent(const char *commerceEventJSON);
void _LogScreen(const char *screenName);
void _SetATTStatus(int status, double timestamp)

void _LeaveBreadcrumb(const char *breadcrumbName);

void _Upload(void);
void _Destroy(void);


char* _Identity_Identify(const char *identityApiRequestJSON);
char* _Identity_Login(const char *identityApiRequestJSON);
char* _Identity_Logout(const char *identityApiRequestJSON);
char* _Identity_Modify(const char *identityApiRequestJSON);

void _Identity_AddIdentityStateListener(void);
void _Identity_RemoveIdentityStateListener(void);

char* _Identity_GetCurrentUser(void);
char* _Identity_GetUser(const char *mpid);

char* _User_SetUserAttribute(const char *mpid, const char *key, const char *value);
char* _User_SetUserAttributes(const char *mpid, const char *attributesJSON);
char* _User_SetUserTag(const char *mpid, const char *tag);
char* _User_RemoveUserAttribute(const char *mpid, const char *key);
char* _User_GetUserAttributes(const char *mpid);

char* _User_GetUserIdentities(const char *mpid);
char* _User_GetCurrentUserMpid(const char *mpid);

#endif
