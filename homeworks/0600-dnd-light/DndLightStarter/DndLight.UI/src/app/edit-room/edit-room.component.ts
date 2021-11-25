import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DndApiClientService } from '../dnd-api-client.service';
import { ItemType } from '../model/itemType';
import { NewRoomDetails } from '../model/newRoomDetails';
import { RoomDetailsResult } from '../model/roomDetailsResult';
import { RoomSummary } from '../model/roomSummary';

@Component({
  selector: 'app-edit-room',
  templateUrl: './edit-room.component.html',
  styleUrls: ['./edit-room.component.css']
})
export class EditRoomComponent implements OnInit {
  id!: number;
  room?: RoomDetailsResult;

  constructor(private apiClient: DndApiClientService, route: ActivatedRoute, private router: Router) {
    // Note: This is how you can get a parameter from a route (in this case the room's id)
    route.paramMap.subscribe(p => this.id = parseInt(p.get('id') ?? ''));
  }

  ngOnInit(): void {
    throw new Error('Not implemented yet');
  }

  save() {
    throw new Error('Not implemented yet');
  }
}
