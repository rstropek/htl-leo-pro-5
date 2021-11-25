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
  rooms?: RoomSummary[];

  constructor(private apiClient: DndApiClientService, route: ActivatedRoute, private router: Router) {
    // Note: This is how you can get a parameter from a route (in this case the room's id)
    route.paramMap.subscribe(p => this.id = parseInt(p.get('id') ?? ''));
  }

  ngOnInit(): void {
    this.apiClient.getRooms().subscribe(data => this.rooms = data);
    this.apiClient.getRoomDetails(this.id).subscribe(data => this.room = data);
  }

  addDoor() {
    this.room?.doors.push({
      id: 0,
      linkedRoomId: this.rooms![0].id,
      description: '',
      initiallyLocked: false,
    });
  }

  deleteDoor(ix: number) {
    this.room?.doors.splice(ix, 1);
  }
  
  addItem() {
    this.room?.items.push({
      id: 0,
      description: '',
      itemType: ItemType.IronKey,
    });
  }

  deleteItem(ix: number) {
    this.room?.items.splice(ix, 1);
  }
  
  addMonster() {
    this.room?.monsters.push({
      id: 0,
      description: '',
      lifePower: 1,
      attackStrength: 1,
      armorStrength: 1,
      attacksOnEntry: false
    });
  }

  deleteMonster(ix: number) {
    this.room?.monsters.splice(ix, 1);
  }

  save() {
    // Note: We can safely cast `RoomDetailsResult` to `NewRoomDetails` as `NewRoomDetails` contains
    //       all fields of `RoomDetailsResult` (even a few additional ones that will be ignored by our server).
    this.apiClient.updateRoom(this.id, <NewRoomDetails>this.room).subscribe(() => this.router.navigate(['room-list']));
  }
}
