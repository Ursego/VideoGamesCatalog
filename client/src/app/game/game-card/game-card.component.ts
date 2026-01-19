import { Component, computed, inject, OnInit } from "@angular/core";
import { toSignal } from "@angular/core/rxjs-interop";
import { Store } from "@ngrx/store";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { CrudOperation, DbBoolean, IDropdownEntry } from "../../util/util";
import { IGameDto } from "../game.model";
import {
	CleanUpAfterSaveAction,
	InsertGameAction,
	UpdateGameAction
} from "../game.actions";
import {
	selectGameToSave,
	selectCrudOperationToPerform,
	selectGameLoaded,
	selectGameCategoryList,
	selectAgeRatingList
} from "../game.selectors";
import { CommonModule } from "@angular/common";

@Component({
	selector: "app-game-card",
	standalone: true,
	imports: [CommonModule, ReactiveFormsModule],
	templateUrl: "./game-card.component.html"
})
export class GameCardComponent implements OnInit {
	private readonly store = inject(Store);
	private readonly activeModal = inject(NgbActiveModal);

	readonly gameToSaveSignal = toSignal(this.store.select(selectGameToSave), { initialValue: null });
	readonly crudOperationToPerformSignal = toSignal(this.store.select(selectCrudOperationToPerform), { initialValue: null });
	readonly gameLoadedSignal = toSignal(this.store.select(selectGameLoaded), { initialValue: false });
	readonly gameCategoryListSignal = toSignal(this.store.select(selectGameCategoryList), { initialValue: [] as IDropdownEntry[] });
	readonly ageRatingListSignal = toSignal(this.store.select(selectAgeRatingList), { initialValue: [] as IDropdownEntry[] });

	readonly gameForm = new FormGroup({
		id: new FormControl<number>(0, { nonNullable: true }),
		name: new FormControl<string>("", { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] }),
		description: new FormControl<string | null>(null, { validators: [Validators.maxLength(2000)] }),
		gameCategoryId: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
		ageRatingId: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
		releaseDate: new FormControl<string | null>(null),
		coverImageUrl: new FormControl<string | null>(null, { validators: [Validators.maxLength(500), Validators.pattern(/^(https?:\/\/).+/i)] }),
		isActive: new FormControl<DbBoolean>(DbBoolean.Yes, { nonNullable: true })
	});

	readonly enableSaveButtonSignal = computed(() => this.gameLoadedSignal() && this.gameForm.dirty && this.gameForm.valid );

	public readonly dbBoolean = DbBoolean; // exposure the enum to the HTML template

	ngOnInit(): void {
		this.gameForm.controls["id"].disable({ emitEvent: false });
	}

	onCloseClick(): void {
		if (this.gameForm.dirty && !confirm("Discard changes?")) return;
		this.store.dispatch(CleanUpAfterSaveAction());
		this.activeModal.close();
	}

	onSaveClick(): void {
		const game = this.gameForm.getRawValue() as IGameDto;

		switch (this.crudOperationToPerformSignal()) {
			case CrudOperation.Insert:
				this.store.dispatch(InsertGameAction({ game: game }));
				break;
			case CrudOperation.Update:
				this.store.dispatch(UpdateGameAction({ game: game }));
				break;
			default:
				throw new Error("crudOperationToPerformSignal must be CrudOperation.Insert or CrudOperation.Update.");
		}
	}
}
