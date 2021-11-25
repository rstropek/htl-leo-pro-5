import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DndApiClientService } from '../dnd-api-client.service';
import { NewRoom } from '../model/newRoom';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent {

  room: NewRoom = {
    description: '',
  }

  constructor(private apiClient: DndApiClientService, private router: Router) { }

  save() {
    // Note: Use `Router` to navigate to other component in code.
    this.apiClient.addRoom(this.room).subscribe(() => this.router.navigate(['room-list']));
  }
}
