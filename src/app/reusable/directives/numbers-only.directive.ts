import { Directive, HostListener,Input } from "@angular/core";
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[numbersOnly]'
})
export class NumbersOnlyDirective {

  constructor(private ngControl: NgControl) { }

  @HostListener('input', ['$event']) onInput(event): void {
    const value:string = event.target.value;
    const newValue:string = parseInt(value).toString() || "";
    this.ngControl.control.setValue(newValue);
    event.preventDefault();
    
  }
  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    event.preventDefault();
    const pastedInput: string = event.clipboardData
      .getData('text/plain')
      .replace(/\D/g, ''); // get a digit-only string
    document.execCommand('insertText', false, pastedInput);
  }
  // @HostListener('drop', ['$event'])
  // onDrop(event: DragEvent) {
  //   event.preventDefault();
  //   const textData = event.dataTransfer
  //     .getData('text').replace(/\D/g, '');
  //   this.ngControl .focus();
  //   document.execCommand('insertText', false, textData);
  // }
}