// WARNING
// This file has been generated automatically by Xamarin Studio Enterprise to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>


@interface ViewController : UIViewController {
	UITextField *_messageTextField;
	UITextField *_receiverNumberTextField;
}
@property (retain, nonatomic) IBOutlet UITextView *consoleTextView;

@property (nonatomic, retain) IBOutlet UITextField *messageTextField;

@property (nonatomic, retain) IBOutlet UITextField *receiverNumberTextField;

- (IBAction)SendMessage:(id)sender;

@end
