import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap } from "rxjs";
import {
	CleanUpAfterSaveAction,
	SelectGameListAction,
	SelectGameListSuccessAction,
	SelectGameListFailureAction,
	SwitchToInsertModeAction,
	InsertGameAction,
	InsertGameSuccessAction,
	InsertGameFailureAction,
	SwitchToUpdateModeAction,
	UpdateGameAction,
	UpdateGameSuccessAction,
	UpdateGameFailureAction,
	DeleteGameAction,
	DeleteGameSuccessAction,
	DeleteGameFailureAction,
	SelectGameCategoryListAction,
	SelectGameCategoryListSuccessAction,
	SelectGameCategoryListFailureAction,
	SelectAgeRatingListAction,
	SelectAgeRatingListSuccessAction,
	SelectAgeRatingListFailureAction
} from "./game.actions";
import { GameService } from "./game.service";

@Injectable()
export class GameEffects {
	private readonly actions: Actions<any> = inject(Actions);
	private readonly gameService: GameService = inject(GameService);

	resetContext$ = createEffect(() =>
		this.actions.pipe(
			ofType(CleanUpAfterSaveAction),
			map(() => ({ type: "[Game] Noop" }))
		)
	);

	selectGameList$ = createEffect(() =>
		this.actions.pipe(
			ofType(SelectGameListAction),
			switchMap(({ gameName }) =>
				this.gameService.getGameList(gameName).pipe(
					map(gameList => SelectGameListSuccessAction({ gameList })),
					catchError(error => of(SelectGameListFailureAction({ error })))
				)
			)
		)
	);

	openGameCardForInsert$ = createEffect(() =>
		this.actions.pipe(
			ofType(SwitchToInsertModeAction),
			map(() => ({ type: "[Game] Noop" }))
		)
	);

	insertGame$ = createEffect(() =>
		this.actions.pipe(
			ofType(InsertGameAction),
			switchMap(({ game: game }) =>
				this.gameService.insertGame(game).pipe(
					map(savedGame => InsertGameSuccessAction({ game: savedGame })),
					catchError(error => of(InsertGameFailureAction({ error })))
				)
			)
		)
	);

	openGameCardForUpdate$ = createEffect(() =>
		this.actions.pipe(
			ofType(SwitchToUpdateModeAction),
			map(() => ({ type: "[Game] Noop" }))
		)
	);

	updateGame$ = createEffect(() =>
		this.actions.pipe(
			ofType(UpdateGameAction),
			switchMap(({ game }) =>
				this.gameService.updateGame(game).pipe(
					map(savedGame => UpdateGameSuccessAction({ game: savedGame })),
					catchError(error => of(UpdateGameFailureAction({ error })))
				)
			)
		)
	);

	deleteGame$ = createEffect(() =>
		this.actions.pipe(
			ofType(DeleteGameAction),
			switchMap(({ gameId }) =>
				this.gameService.deleteGame(gameId).pipe(
					map(() => DeleteGameSuccessAction({ gameId })),
					catchError(error => of(DeleteGameFailureAction({ error })))
				)
			)
		)
	);
    
	selectGameCategoryList$ = createEffect(() =>
        this.actions.pipe(
            ofType(SelectGameCategoryListAction),
            switchMap(() =>
                this.gameService.getGameCategoryList().pipe(
                    map(gameCategoryList => SelectGameCategoryListSuccessAction({ gameCategoryList })),
                    catchError(error => of(SelectGameCategoryListFailureAction({ error })))
                )
            )
        )
    );
    
    selectAgeRatingList$ = createEffect(() =>
        this.actions.pipe(
            ofType(SelectAgeRatingListAction),
            switchMap(() =>
                this.gameService.getAgeRatingList().pipe(
                    map(ageRatingList => SelectAgeRatingListSuccessAction({ ageRatingList })),
                    catchError(error => of(SelectAgeRatingListFailureAction({ error })))
                )
            )
        )
    );
}
