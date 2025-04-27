import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-form-input',
  templateUrl: './form-input.component.html',
  styleUrl: './form-input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponent),
      multi: true
    }
  ]
})
export class FormInputComponent implements ControlValueAccessor {

  constructor(){}

  value: string = '';
  isDisabled: boolean = false
  changeFunc: (value: string) => void = () => {};
  touchedFunc: () => void = () => {}
  
  
  writeValue(value: string): void {
    this.value = value
  }
  registerOnChange(fn: any): void {
    this.changeFunc = fn
  }
  registerOnTouched(fn: any): void {
   this.touchedFunc = fn
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled
  }

}
