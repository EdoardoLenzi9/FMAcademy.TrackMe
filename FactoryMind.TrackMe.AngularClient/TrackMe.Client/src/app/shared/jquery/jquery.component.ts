import { Component, OnInit, ElementRef} from '@angular/core';
declare var jQuery:any;

@Component({
  selector: 'app-jquery',
  templateUrl: './jquery.component.html',
  styleUrls: ['./jquery.component.css']
})
export class JQueryComponent implements OnInit {

  constructor(private _el : ElementRef) { }

  ngOnInit() {
		jQuery(this._el.nativeElement).find('button').on('click', function()
		{
			console.log("Jquery funziona");
		})
  }
}
