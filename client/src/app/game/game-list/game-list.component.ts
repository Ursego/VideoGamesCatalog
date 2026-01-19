import { CommonModule } from "@angular/common";
import { Component, computed, inject, Signal } from "@angular/core";
import { Store } from "@ngrx/store";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { IGameDto } from "../game.model";
import { GameCardComponent } from "../game-card/game-card.component";
import { selectGameList, selectGameListLoaded, selectGameCategoryList, selectAgeRatingList, selectHighlightedGame } from "../game.selectors";
import {
	SetHighlightedGameAction,
	CleanUpAfterSaveAction,
	SelectGameListAction,
	SelectGameCategoryListAction,
	SelectAgeRatingListAction,
	DeleteGameAction,
	SwitchToInsertModeAction,
	SwitchToUpdateModeAction
} from "../game.actions";

@Component({
	selector: "app-game-list",
	standalone: true,
	imports: [CommonModule],
	templateUrl: "./game-list.component.html"
})
export class GameListComponent {
	private readonly store = inject(Store);
	private readonly ngbModal = inject(NgbModal);

	readonly highlightedGameSignal = this.store.selectSignal(selectHighlightedGame);
	readonly gameListLoadedSignal = this.store.selectSignal(selectGameListLoaded);
	readonly gameListSignal = this.store.selectSignal(selectGameList);
	readonly gameCategoryListSignal = this.store.selectSignal(selectGameCategoryList);
	readonly ageRatingListSignal = this.store.selectSignal(selectAgeRatingList);

	constructor() {
		this.store.dispatch(SelectGameCategoryListAction());
		this.store.dispatch(SelectAgeRatingListAction());
	}

	getHighlightedGameId(): number | null {
		const highlightedGame = this.highlightedGameSignal();
		return highlightedGame ? highlightedGame.id : null;
	}

	highlightedGameExists(): boolean {
		const highlightedGame = this.highlightedGameSignal();
		return (highlightedGame !== null);
	}

	onSearchButtonClick(gameName: string): void {
		const trimmedGameName = gameName.trim();
		if (trimmedGameName === "" && !confirm("You didn't enter a game name, so all games will be shown. Do you want to proceed?")) return;
		this.store.dispatch(SelectGameListAction({ gameName: trimmedGameName }));
	}

	onRowClick(highlightedGame: IGameDto): void {
		this.store.dispatch(SetHighlightedGameAction({ highlightedGame: highlightedGame }));
	}

	onAddButtonClick(): void {
		this.store.dispatch(SwitchToInsertModeAction());
		this.openGameCard();
	}

	onEditButtonClick(): void {
		const gameToSave = this.highlightedGameSignal()!!; // must be not null - Edit button is enabled only when a game is highlighted
		this.store.dispatch(SwitchToUpdateModeAction({ gameToSave: gameToSave }));
		this.openGameCard();
	}

	onDeleteButtonClick(): void {
		if (!window.confirm("Delete the selected game?")) return;
		const highlightedGameId = this.getHighlightedGameId()!!; // must be not null - Delete button is enabled only when a game is highlighted
		this.store.dispatch(DeleteGameAction({ gameId: highlightedGameId}));
	}

	getGameCategoryDescription(gameCategoryId: number): string {
		return this.gameCategoryDescriptionById().get(gameCategoryId) ?? "";
	}

	getAgeRatingDescription(ageRatingId: number): string {
		return this.ageRatingDescriptionById().get(ageRatingId) ?? "";
	}

	private openGameCard(): void {
		const modalRef = this.ngbModal.open(GameCardComponent, { size: "lg", backdrop: "static", keyboard: false });
		modalRef.result.finally(() => {
			this.store.dispatch(CleanUpAfterSaveAction());
		});
	}

	private readonly gameCategoryDescriptionById: Signal<Map<number, string>> = computed(() => {
		const map = new Map<number, string>();
		for (const pair of this.gameCategoryListSignal()) map.set(pair.id, pair.description);
		return map;
	});

	private readonly ageRatingDescriptionById: Signal<Map<number, string>> = computed(() => {
		const map = new Map<number, string>();
		for (const pair of this.ageRatingListSignal()) map.set(pair.id, pair.description);
		return map;
	});
}
