import {NgModule} from '@angular/core';
import {MatLegacyButtonModule as MatButtonModule} from '@angular/material/legacy-button';
import { MatLegacyFormFieldModule as MatFormFieldModule } from '@angular/material/legacy-form-field';
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input';
import { MatLegacyAutocompleteModule as MatAutocompleteModule } from '@angular/material/legacy-autocomplete';
import { MatLegacyTableModule as MatTableModule } from '@angular/material/legacy-table';
import {MatLegacyCardModule as MatCardModule} from '@angular/material/legacy-card';
import { MatSortModule } from '@angular/material/sort'



import {MatLegacyPaginatorModule as MatPaginatorModule} from '@angular/material/legacy-paginator'; 
const modules =[MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatTableModule,
    MatPaginatorModule,
    MatCardModule,
    MatSortModule
    
];

@NgModule({
    imports:[...modules],
    exports:[...modules]
})
export class MaterialModule{};
