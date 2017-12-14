import { Directive, ElementRef, Input, OnInit } from '@angular/core';


@Directive({
  selector: '[appInputTitle]'
})
export class InputTitleDirective{

  constructor(el: ElementRef) {
       el.nativeElement.style.backgroundColor = 'yellow';

    }

}
