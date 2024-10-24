import { Injectable } from '@angular/core';
import { GridInputDto } from '../../../../shared/model/AppModel';

@Injectable({
  providedIn: 'root'
})
export class GridServiceService {
  constructor() { }

  handleSortData(sortInput: any, gridInput: GridInputDto) {
    const newSortMeta = sortInput.gridSortMeta.map((meta: any) => ({ ...meta }));

    let hasChanges = false;

    // Check if the length of sortMeta differs, indicating a change
    if (gridInput.sortMeta?.length !== newSortMeta.length) {
      hasChanges = true;
    } else {
      // Compare each field and order between the existing and new sort meta
      for (let i = 0; i < gridInput.sortMeta!.length; i++) {
        const oldSort = gridInput.sortMeta![i];
        const newSort = newSortMeta[i];

        // Check if field or order has changed
        if (oldSort.field !== newSort.field || oldSort.order !== newSort.order) {
          hasChanges = true;
          break;
        }
      }
    }

    if (hasChanges) {
      return { hasChanges: hasChanges, newSortMeta: newSortMeta };
    }

    return { hasChanges: hasChanges, newSortMeta: newSortMeta };

  }
}
