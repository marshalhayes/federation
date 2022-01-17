import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UscfPlayer } from '../../../models/players/uscf/uscfPlayer';

@Component({
  selector: 'app-player-uscf-profile',
  templateUrl: './uscfPlayerProfile.component.html',
})
export class UscfPlayerProfileComponent {
  readonly player: UscfPlayer;
  readonly today = new Date(Date.now());

  constructor(activatedRoute: ActivatedRoute) {
    this.player = activatedRoute.snapshot.data.player as UscfPlayer;
  }
}
