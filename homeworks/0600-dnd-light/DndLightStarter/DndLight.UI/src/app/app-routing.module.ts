import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddRoomComponent } from './add-room/add-room.component';
import { EditRoomComponent } from './edit-room/edit-room.component';
import { RoomListComponent } from './room-list/room-list.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'room-list' },
  { path: 'room-list', component: RoomListComponent },
  { path: 'add-room', component: AddRoomComponent },
  // Note: The route contains a parameter (`id`)
  { path: 'edit-room/:id', component: EditRoomComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
