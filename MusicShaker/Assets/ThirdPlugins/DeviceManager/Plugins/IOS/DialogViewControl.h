//
//  DialogViewControl.h
//  Unity-iPhone
//
//  Created by CAT on 2018/5/14.
//

#ifndef DialogViewControl_h
#define DialogViewControl_h

@interface DialogViewController : UIViewController<UIAlertViewDelegate>
- (void)showAlert:(const char *)title message:(const char*)message okButtonTitle:(const char*)okButton cancelButtonTitle:(const char*)cancelButton;
@end
#endif /* DialogViewControl_h */
