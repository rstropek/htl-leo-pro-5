import { Component } from '@angular/core';
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
    throw new Error('Not implemented yet');
  }
}
