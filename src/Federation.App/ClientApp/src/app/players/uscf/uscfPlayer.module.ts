import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UscfPlayerProfileComponent } from './uscfPlayerProfile.component';
import { RouterModule } from '@angular/router';
import { UscfPlayerResolver } from './uscfPlayer.resolver';
import { FormsModule } from '@angular/forms';

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
    FormsModule,
  ],
  declarations: [UscfPlayerProfileComponent],
  providers: [UscfPlayerResolver],
})
export class UscfPlayerModule {}
