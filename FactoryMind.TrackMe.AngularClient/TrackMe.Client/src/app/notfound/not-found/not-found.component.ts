import { Component} from '@angular/core';
import { RoutingService } from "../../routing.service";

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent {
  routing:RoutingService;
  constructor(public routingSvc: RoutingService) {
    this.routing =routingSvc;
  }

}
