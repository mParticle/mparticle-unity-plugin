#ifndef mParticle_mParticleUnity_h
#define mParticle_mParticleUnity_h

int _GetEnvironment();
void _SetOptOut(Boolean optOut);

void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON);
void _LogCommerceEvent(const char *commerceEventJSON);
void _LogScreen(const char *screenName, const char *eventInfoJSON);

void _LeaveBreadcrumb(const char *breadcrumbName, const char *eventInfoJSON);

int _IncrementUserAttribute(const char *key, int incrementValue);
void _Logout();
void _SetUserAttribute(const char *key, const char *value);
void _SetUserAttributeArray(const char *key, const char *values[], int length);
void _SetUserIdentity(const char *identity, unsigned int identityType);
void _SetUserTag(const char *tag);
void _RemoveUserAttribute (const char *key);

#endif
