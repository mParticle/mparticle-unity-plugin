//
//  MParticleUnityException.h
//  mParticle
//
//  Created by Dalmo Cirne on 5/5/14.
//  Copyright (c) 2014 mParticle. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MParticleUnityException : NSException

@property (nonatomic, strong, readonly) NSArray *callStack;

- (instancetype)initWithName:(NSString *)aName reason:(NSString *)aReason callStack:(NSString *)aCallStack;
- (NSArray *)callStackSymbols;

@end
