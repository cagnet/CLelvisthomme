import { HttpErrorResponse } from '@angular/common/http';

export interface ApiError {
    status: number;
    message: string;
    code: string | null;
    fieldErrors: Record<string, string[]>;
}

// 400 body sent by ASP.NET (ValidationProblemDetails)
interface ValidationProblem {
    title?: string;
    errors: Record<string, string[]>;
}

// 409 body sent by the API for business rule violations
interface ConflictProblem {
    code: string;
    detail: string;
}

function isValidationProblem(body: unknown): body is ValidationProblem {
    return typeof body === 'object' && body !== null && 'errors' in body;
}

function isConflictProblem(body: unknown): body is ConflictProblem {
    return typeof body === 'object' && body !== null && 'code' in body && 'detail' in body;
}

// The API returns PascalCase keys ("Name", "Amount"), the form controls are camelCase
function toFieldName(key: string): string {
    const name = key.replace(/^\$\./, '');
    return name.charAt(0).toLowerCase() + name.slice(1);
}

export function toApiError(error: HttpErrorResponse): ApiError {
    const apiError: ApiError = { status: error.status, message: '', code: null, fieldErrors: {} };

    if (error.status === 0) {
        apiError.message = 'Unable to reach the API. Check that the server is running.';
    } else if (isValidationProblem(error.error)) {
        for (const [key, messages] of Object.entries(error.error.errors)) {
            apiError.fieldErrors[toFieldName(key)] = messages;
        }
        apiError.message = 'Some fields are invalid.';
    } else if (isConflictProblem(error.error)) {
        apiError.code = error.error.code;
        apiError.message = error.error.detail;
    } else if (error.status === 404) {
        apiError.message = 'The requested resource was not found.';
    } else {
        apiError.message = `Unexpected error (${error.status} ${error.statusText}).`;
    }

    return apiError;
}
