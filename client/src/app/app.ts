import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { RouterLink, RouterOutlet } from "@angular/router";

@Component({
	selector: "app-root",
	standalone: true,
	imports: [RouterOutlet],
	template: "<router-outlet></router-outlet>"
})
export class AppComponent {
}

@Component({
	selector: "app-home",
	standalone: true,
	imports: [CommonModule, RouterLink],
	templateUrl: "./app.html",
	styleUrl: "./app.scss"
})
export class AppHome {
}