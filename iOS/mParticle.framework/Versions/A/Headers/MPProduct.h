//
//  MPProduct.h
//
//  Copyright 2014. mParticle, Inc. All Rights Reserved.
//

#import <Foundation/Foundation.h>

// MPProduct
extern NSString *const kMPProductName;
extern NSString *const kMPProductAffiliation;
extern NSString *const kMPProductSKU;
extern NSString *const kMPProductUnitPrice;
extern NSString *const kMPProductQuantity;
extern NSString *const kMPProductRevenue;
extern NSString *const kMPProductTax;
extern NSString *const kMPProductShipping;
extern NSString *const kMPProductTransactionId;
extern NSString *const kMPProductCategory;
extern NSString *const kMPProductCurrency;

/**
 This class can be used to log a purchase transaction or to pass a product as a parameter to an event.
 Since this class extends NSMutableDictionary, other key/value pairs can be specified, in addition to the
 ones listed as class properties.
 */
@interface MPProduct : NSObject

/**
 An entity with which the transaction should be affiliated (e.g. a particular store). If nil, mParticle will use an empty string
 */
@property (nonatomic, strong) NSString *affiliation;

/**
 A category to which the product belongs
 */
@property (nonatomic, strong) NSString *category;

/**
 The currency of a transaction. If not specified, mParticle will use "USD"
 */
@property (nonatomic, strong) NSString *currency;

/**
 The name of the product
 */
@property (nonatomic, strong) NSString *name;

/**
 SKU of a product
 */
@property (nonatomic, strong) NSString *sku;

/**
 A unique ID representing the transaction. This ID should not collide with other transaction IDs. If not specified, mParticle will generate a random id with 20 characters
 */
@property (nonatomic, strong) NSString *transactionId;

/**
 The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0. Default value is zero
 */
@property (nonatomic, readwrite) double revenueAmount;

/**
 The total cost of shipping for a transaction. If free or non-applicable use 0. Default value is zero
 */
@property (nonatomic, readwrite) double shippingAmount;

/**
 The total tax for a transaction. If free or non-applicable use 0. Default value is zero
 */
@property (nonatomic, readwrite) double taxAmount;

/**
 The price of a product. If free or non-applicable use 0. Default value is zero
 */
@property (nonatomic, readwrite) double unitPrice;

/**
 The quantity of a product. If non-applicable use 0. Default value is zero
 */
@property (nonatomic, readwrite) NSInteger quantity;

/**
 Designated initialiser.
 @param name The name of the product
 @param category A category to which the product belongs
 @param quantity The quantity of a product. If non-applicable use 0
 @param revenueAmount The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0
 */
- (instancetype)initWithName:(NSString *)name category:(NSString *)category quantity:(NSInteger)quantity revenueAmount:(double)revenueAmount;

- (id)objectForKeyedSubscript:(NSString *const)key;
- (void)setObject:(id)obj forKeyedSubscript:(NSString *)key;
- (NSArray *)allKeys;
- (NSUInteger)count;

@end
