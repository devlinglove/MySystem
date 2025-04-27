import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, FormControl, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-form-input-email',
  templateUrl: './form-input-email.component.html',
  styleUrl: './form-input-email.component.scss',
  providers: [
      {
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => FormInputEmailComponent),
        multi: true
      }
  ]
})
export class FormInputEmailComponent implements ControlValueAccessor {

  constructor(){}

  @Input() parentForm!: FormGroup
  @Input() formFieldName: string = ''

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

  get formField(): FormControl {
    return this.parentForm.get(this.formFieldName) as FormControl;
  }

 onChange(event: Event){
  let string = (<HTMLInputElement>event.target).value

  this.changeFunc(string)
 }




}
