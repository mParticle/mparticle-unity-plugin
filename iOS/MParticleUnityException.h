#import <Foundation/Foundation.h>

@interface MParticleUnityException : NSException

@property (nonatomic, strong, readonly) NSArray *callStack;

- (instancetype)initWithName:(NSString *)aName reason:(NSString *)aReason callStack:(NSString *)aCallStack;
- (NSArray *)callStackSymbols;

@end
