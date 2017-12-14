
/*
https://github.com/akserg/ng2-toasty
*/



import { Component, OnInit, Input } from '@angular/core';
import { ToastyService, ToastyConfig, ToastOptions, ToastData } from 'ng2-toasty';

import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
	selector: 'app-toast2',
	templateUrl: './toast.component.html',
	styleUrls: ['./toast.component.css'],
	animations: [
		trigger('status', [
			state('inactive', style({ transform: 'translateX(0) scale(1)', opacity: 1 })),

			transition('void => inactive', [
				style({ transform: 'translateX(-100%) scale(1)', opacity: 0 }),
				animate(200)
			]),
			transition('inactive => void', [
				animate(200, style({ transform: 'translateX(100%) scale(1)', opacity: 0 }))
			]),
		])
	]
})
export class ToastComponent2 {
status:string="void";
	constructor(private toastyService: ToastyService, private toastyConfig: ToastyConfig) {
		// Assign the selected theme name to the `theme` property of the instance of ToastyConfig.
		// Possible values: default, bootstrap, material
		this.toastyConfig.theme = 'material';
	}
	addToast(Type: string, Title: string, Msg: string, Time: number) {
		// Or create the instance of ToastOptions
		var toastOptions: ToastOptions = {
			title: Title,
			msg: Msg,
			showClose: true,
			timeout: Time,
			theme: 'material',
/*
			onAdd: (toast:ToastData) => {
					console.log('Toast ' + toast.id + ' has been added!');
					status="inactive";
			},
			onRemove: function(toast:ToastData) {
					console.log('Toast ' + toast.id + ' has been removed!');
					status="void";
			}*/
		};
		switch (Type) {
			case "info":
				this.toastyService.info(toastOptions);
				break;
			case "success":
				this.toastyService.success(toastOptions);
				break;
			case "wait":
				this.toastyService.wait(toastOptions);
				break;
			case "error":
				this.toastyService.error(toastOptions);
				break;
			case "warning":
				this.toastyService.warning(toastOptions);
				break;
			default:
				this.toastyService.default('No Type attribute set');
				break;
		};


	}
}



