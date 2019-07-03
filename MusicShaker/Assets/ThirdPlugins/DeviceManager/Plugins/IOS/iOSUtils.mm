#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>
#import <AdSupport/ASIdentifierManager.h>
#import "DialogViewControl.h"

#if defined (__cplusplus)
extern "C" {
#endif
    bool openURL(const char *scheme)
    {
        NSString *urlString=[NSString stringWithCString:scheme encoding:NSUTF8StringEncoding];
        NSURL *url = [NSURL URLWithString:urlString];
        NSLog(@"open scheme: %@", urlString);
        if ([[UIApplication sharedApplication] canOpenURL:url]) {
            [[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
            return true;
        }else{
            NSLog(@"Not Install App,or Not Registeer LSApplicatonQueriesSchemes.");
        }
        return false;
    }
    
    void setBrightness(float brightness)
    {
        [[UIScreen mainScreen] setBrightness : brightness];
    }
    
    float getBrightness()
    {
        return [[UIScreen mainScreen] brightness];
    }
    
    void setVolume(float volume)
    {
        MPMusicPlayerController *musicPlayer = [MPMusicPlayerController applicationMusicPlayer];
        [musicPlayer setVolume:volume];
    }
    
    float getVolume()
    {
        MPMusicPlayerController *musicPlayer = [MPMusicPlayerController applicationMusicPlayer];
        NSNumber *nowVolum = [NSNumber numberWithFloat:musicPlayer.volume];
        return nowVolum.floatValue;
    }
    
    const char * getDeviceID()
    {
        NSString *adId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
        return strdup([adId UTF8String]);
    }
    
    
    
    typedef void (*AlertViewDelegate)(int buttonIndex);
    
    AlertViewDelegate g_alertViewDelegate;
    
    void _showDialog(const char* title, const char* message, const char* okButton, const char* cancelButton, AlertViewDelegate alertViewDelegate)
    {
        g_alertViewDelegate = alertViewDelegate;
        
        [[[DialogViewController alloc] init] showAlert:title message:message okButtonTitle:okButton cancelButtonTitle:cancelButton];
    }
    
    @implementation DialogViewController
- (void)showAlert:(const char *)title message:(const char*)message okButtonTitle:(const char*)okButton cancelButtonTitle:(const char*)cancelButton{
    
    NSString* cancelButtonTitle = nil;
    if(strcmp(cancelButton, "") != 0)
        cancelButtonTitle = [NSString stringWithUTF8String:cancelButton];
    
    NSString* okButtonTitle = nil;
    if(strcmp(okButton, "") != 0)
        okButtonTitle = [NSString stringWithUTF8String:okButton];
    
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:[NSString stringWithUTF8String:title]
                                                    message:[NSString stringWithUTF8String:message]
                                                   delegate:self
                                          cancelButtonTitle:cancelButtonTitle
                                          otherButtonTitles:okButtonTitle, nil];
    [alert show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
    
    NSLog(@"clickedButtonAtIndex %ld", buttonIndex);
    if(g_alertViewDelegate != NULL)
        g_alertViewDelegate((int)buttonIndex);
}
@end
    
    
    
    
#if defined (__cplusplus)
}
#endif
