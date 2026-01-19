import { Routes } from "@angular/router";
import { AppHome } from "./app";

export const routes: Routes = [
	{ path: "", component: AppHome },
	// Would require a static import { GameListComponent } from "./game/game-list/game-list.component":
	// { path: "games", component: GameListComponent },
	// Lazy-loading, avoids importing the component up-front and reduces initial bundle size
	// (just for demonstration; of course, in real apps, it's for pages with a low probability of being opened):
	{ path: "games", loadComponent: () => import("./game/game-list/game-list.component").then(m => m.GameListComponent) },
	{ path: "**", redirectTo: "" }
];