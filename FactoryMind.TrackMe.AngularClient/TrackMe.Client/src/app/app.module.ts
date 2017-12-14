import { HttpCallSvcService } from './http-call-svc.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { ServerService } from './Services/server.service';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from "app/notfound/not-found/not-found.component";
import { MainComponent } from "app/main/main/main.component";
import { RoutingService } from "app/routing.service";
import { LoginModule } from "app/login/login.module";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule } from '@angular/http';
@NgModule({
  declarations: [
    AppComponent,
		NotFoundComponent
  ],
  imports: [
		HttpModule,
		BrowserAnimationsModule,
    RouterModule.forRoot(
      [
        { path: '', redirectTo: 'login', pathMatch: 'full' },
        { path: 'login', loadChildren: "app/login/login.module#LoginModule"},
        { path: 'main', loadChildren: "app/main/main.module#MainModule"},
        { path: 'not-found', component: NotFoundComponent },
        { path: '**', component: NotFoundComponent },
      ]
    )
  ],
  providers: [RoutingService,ServerService,HttpCallSvcService],
  bootstrap: [AppComponent]
})
export class AppModule { }
