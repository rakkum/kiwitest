import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-book-dialog',
  styleUrls: ['./book-dialog.component.css'],
  templateUrl: './book-dialog.component.html'
})
export class BookDialogComponent {

  constructor(public dialogRef: MatDialogRef<BookDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}

export interface DialogData {
  title: string;
  description: string;
  publishDate: Date;
}
