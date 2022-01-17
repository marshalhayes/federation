import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UscfPlayerProfileComponent } from './uscfPlayerProfile.component';
import { RouterModule } from '@angular/router';
import { UscfPlayerResolver } from './uscfPlayer.resolver';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: ':uscfId',
        component: UscfPlayerProfileComponent,
        resolve: {
          player: UscfPlayerResolver,
        },
      },
    ]),
  ],
  declarations: [UscfPlayerProfileComponent],
  providers: [UscfPlayerResolver],
})
export class UscfPlayerModule {}
