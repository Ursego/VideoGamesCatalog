import { Injectable, inject, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IDropdownEntry } from "../util/util";
import { IGameDto } from "./game.model";

@Injectable({ providedIn: "root" })
export class GameService {
	private readonly http = inject(HttpClient);
	private readonly platformId = inject(PLATFORM_ID);

	// Browser: relative URLs -> dev proxy can work.
	// SSR/prerender: absolute URLs -> hit the real API directly.
	private readonly apiBaseUrl = isPlatformBrowser(this.platformId) ? "" : "http://localhost:5034";

	getGameList(gameName: string | null): Observable<IGameDto[]> {
		return this.http.get<IGameDto[]>(`${this.apiBaseUrl}/games`, { params: { name: (gameName ?? "").trim() } });
	}

	insertGame(game: IGameDto): Observable<IGameDto> {
		return this.http.post<IGameDto>(`${this.apiBaseUrl}/games`, game);
	}

	updateGame(game: IGameDto): Observable<IGameDto> {
		return this.http.put<IGameDto>(`${this.apiBaseUrl}/games/${game.id}`, game);
	}

	deleteGame(gameId: number): Observable<void> {
		return this.http.delete<void>(`${this.apiBaseUrl}/games/${gameId}`);
	}

	getGameCategoryList(): Observable<IDropdownEntry[]> {
		return this.http.get<IDropdownEntry[]>(`${this.apiBaseUrl}/gamecategories`);
	}

	getAgeRatingList(): Observable<IDropdownEntry[]> {
		return this.http.get<IDropdownEntry[]>(`${this.apiBaseUrl}/ageratings`);
	}
}