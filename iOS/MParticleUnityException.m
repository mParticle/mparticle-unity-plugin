//
//  MParticleUnityException.m
//  mParticle
//
//  Created by Dalmo Cirne on 5/5/14.
//  Copyright (c) 2014 mParticle. All rights reserved.
//

#import "MParticleUnityException.h"

@interface MParticleUnityException() {
    NSString *callStack;
}

@end

@implementation MParticleUnityException

@synthesize callStack = _callStack;

- (instancetype)initWithName:(NSString *)aName reason:(NSString *)aReason callStack:(NSString *)aCallStack {
    self = [super initWithName:aName reason:aReason userInfo:nil];
    if (!self) {
        return nil;
    }
    
    callStack = aCallStack;
    _callStack = nil;
    
    return self;
}

#pragma mark Public methods
- (NSArray *)callStack {
    if (_callStack) {
        return _callStack;
    }

    if (callStack != NULL) {
        _callStack = [callStack componentsSeparatedByString:@"\n"];
    } else {
        _callStack = [super callStackSymbols];
    }
    
    return _callStack;
}

- (NSArray *)callStackSymbols {
    return self.callStack;
}

@end
