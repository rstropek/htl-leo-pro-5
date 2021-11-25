import { RoomSummary } from './roomSummary';

export interface GameSetupSummary { 
    id: number;
    description: string;
    startingRoom: RoomSummary;
    initialLifePower: number;
    initialAttackStrength: number;
    initialArmorStrength: number;
}