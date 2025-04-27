import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormInputComponent } from '../form-input/form-input.component';
import { FormInputEmailComponent } from '../form-input-email/form-input-email.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormInputPasswordComponent } from '../form-input-password/form-input-password.component';



@NgModule({
  declarations: [
    FormInputComponent,
    FormInputEmailComponent,
    FormInputPasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    FormInputComponent,
    FormInputEmailComponent,
    FormInputPasswordComponent
  ]
})
export class SharedModule { }
