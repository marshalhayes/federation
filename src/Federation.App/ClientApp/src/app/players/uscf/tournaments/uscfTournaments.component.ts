import { Component, Input, SimpleChanges } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { UscfPlayerTournament } from '../../../../models/players/uscf/uscfPlayerTournament';
import { switchMap, tap } from 'rxjs/operators';
import { Federation } from '../../../../models/federation';
import { PlayerService } from '../../player.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-uscf-tournaments',
  templateUrl: './uscfTournaments.component.html',
})
export class UscfTournamentsComponent {
  @Input() playerId!: string;

  // Don't hate me
  readonly min = Math.min;

  limitSelection = 10;
  limitOptions = [10, 50, 100];

  tournaments$!: Observable<{
    total: number;
    results: Array<UscfPlayerTournament>;
  } | null>;

  tournamentPaginationSubject$ = new BehaviorSubject({
    offset: 0,
    limit: this.limitSelection,
  });

  tournamentsLoading = false;

  constructor(
    private readonly playerService: PlayerService,
    private readonly sanitizer: DomSanitizer,
  ) {}

  next() {
    const { offset, limit } = this.tournamentPaginationSubject$.value;

    this.tournamentPaginationSubject$.next({
      offset: offset + limit,
      limit,
    });
  }

  previous() {
    const { offset, limit } = this.tournamentPaginationSubject$.value;

    const previous = offset - limit;

    this.tournamentPaginationSubject$.next({
      offset: previous < 0 ? 0 : previous,
      limit,
    });
  }

  getLinkToEvent(eventId: string, playerId: string) {
    return this.sanitizer.bypassSecurityTrustUrl(
      `https://www.uschess.org/msa/XtblMain.php?${eventId}-${playerId}`,
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    this.tournaments$ = this.tournamentPaginationSubject$.pipe(
      tap(() => (this.tournamentsLoading = true)),

      switchMap(({ offset, limit }) =>
        this.playerService.getTournamentsForPlayer(
          changes['playerId'].currentValue || this.playerId,
          Federation.USCF,
          offset,
          limit,
        ),
      ),

      tap(() => (this.tournamentsLoading = false)),
    );
  }
}
