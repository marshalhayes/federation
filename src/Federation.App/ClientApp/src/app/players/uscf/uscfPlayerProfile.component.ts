import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UscfPlayer } from '../../../models/players/uscf/uscfPlayer';
import { PlayerService } from '../player.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { Federation } from '../../../models/federation';
import { UscfPlayerTournament } from '../../../models/players/uscf/uscfPlayerTournament';
import { switchMap, tap } from 'rxjs/operators';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-player-uscf-profile',
  templateUrl: './uscfPlayerProfile.component.html',
})
export class UscfPlayerProfileComponent {
  readonly player: UscfPlayer;
  readonly today = new Date(Date.now());

  // Don't hate me
  readonly min = Math.min;

  limitSelection = 10;
  limitOptions = [10, 50, 100];

  tournaments$: Observable<{
    total: number;
    results: Array<UscfPlayerTournament>;
  } | null>;

  tournamentPaginationSubject$ = new BehaviorSubject({
    offset: 0,
    limit: this.limitSelection,
  });

  tournamentsLoading = false;

  constructor(
    activatedRoute: ActivatedRoute,
    playerService: PlayerService,
    private readonly sanitizer: DomSanitizer,
  ) {
    this.player = activatedRoute.snapshot.data.player as UscfPlayer;

    this.tournaments$ = this.tournamentPaginationSubject$.pipe(
      tap(() => (this.tournamentsLoading = true)),

      switchMap(({ offset, limit }) =>
        playerService.getTournamentsForPlayer(
          this.player.playerId,
          Federation.USCF,
          offset,
          limit,
        ),
      ),

      tap(() => (this.tournamentsLoading = false)),
    );
  }

  getLinkToEvent(eventId: string, playerId: string) {
    return this.sanitizer.bypassSecurityTrustUrl(
      `https://www.uschess.org/msa/XtblMain.php?${eventId}-${playerId}`,
    );
  }

  nextTournaments() {
    const { offset, limit } = this.tournamentPaginationSubject$.value;

    this.tournamentPaginationSubject$.next({
      offset: offset + limit,
      limit,
    });
  }

  previousTournaments() {
    const { offset, limit } = this.tournamentPaginationSubject$.value;

    const previous = offset - limit;

    this.tournamentPaginationSubject$.next({
      offset: previous < 0 ? 0 : previous,
      limit,
    });
  }
}
