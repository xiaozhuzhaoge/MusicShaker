//
//  MyUnityAppController.m
//  OverWrite UnityAppController to apply APP Start From Other APP Call.
//
//  Created by Andy on 2018/4/3.
//  Copyright © 2018年 naocy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UnityAppController.h"

@interface MyUnityAppController : UnityAppController
{
}

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions;

- (void) openURLAfterDelay:(NSURL*) url;

-(BOOL) application:(UIApplication *)application handleOpenURL:(NSURL *)url;

-(BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;

@end
@implementation MyUnityAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    if ([launchOptions objectForKey:UIApplicationLaunchOptionsURLKey]) {
        NSURL *url = [launchOptions objectForKey:UIApplicationLaunchOptionsURLKey];
        
        [self performSelector:@selector(openURLAfterDelay:) withObject:url afterDelay:2];
    }    
    return YES;
}

- (void) openURLAfterDelay:(NSURL*) url
{
    
    UnitySendMessage("MessageReceiver", "OnOpenWithUrl", [[url absoluteString] UTF8String]);
}

-(BOOL) application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    
    UnitySendMessage("MessageReceiver", "OnOpenWithUrl", [[url absoluteString] UTF8String]);    
    return YES;    
}

-(BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation

{
    UnitySendMessage("MessageReceiver", "OnOpenWithUrl", [[url absoluteString] UTF8String]);
    return YES;
}
@end

IMPL_APP_CONTROLLER_SUBCLASS(MyUnityAppController)
