import { ToastComponent2 } from '../shared/toast2/toast.component';
import { UserItemComponent } from './room-page/user-item/user-item.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { NguiMapModule } from '@ngui/map/dist';
import { MainComponent } from './main/main.component';
import { UserComponent } from './main/user/user.component';
import { RoomPageComponent } from './room-page/room-page.component';
import { DndModule } from 'ng2-dnd';
import { Ng2DeviceDetectorModule } from 'ng2-device-detector';
import { ToastyModule } from "ng2-toasty";

@NgModule({
	imports: [
		CommonModule,
		HttpModule,
		JsonpModule,
		FormsModule,
		DndModule.forRoot(),
		ToastyModule.forRoot(),
		Ng2DeviceDetectorModule.forRoot(),
		NguiMapModule.forRoot({ apiUrl: 'https://maps.google.com/maps/api/js?sensor=false' }),
		RouterModule.forChild(
			[
				{ path: '', component: MainComponent },
				{ path: 'rooms', component: RoomPageComponent },
			])
	],
	declarations:
	[
		MainComponent,
		UserComponent,
		RoomPageComponent,
		UserItemComponent,
		ToastComponent2
	],
})
export class MainModule { }
