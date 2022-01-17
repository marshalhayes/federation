import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerService } from './player.service';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: 'uscf',
        loadChildren: () =>
          import('./uscf/uscfPlayer.module').then(m => m.UscfPlayerModule),
      },
    ]),
  ],
  providers: [PlayerService],
})
export class PlayerModule {}
