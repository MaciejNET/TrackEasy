import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { confirmEmail } from "@/api/user-api";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { CheckCircle, XCircle, Loader2 } from "lucide-react";

export default function ConfirmEmail() {
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState<"loading" | "success" | "error">("loading");
  const [errorMessage, setErrorMessage] = useState<string>("");

  useEffect(() => {
    const confirmAccount = async () => {
      const token = searchParams.get("token");
      const email = searchParams.get("email");

      if (!token || !email) {
        setStatus("error");
        setErrorMessage("Missing token or email parameter");
        return;
      }

      try {
        await confirmEmail({ email, token });
        setStatus("success");
      } catch (error) {
        setStatus("error");
        setErrorMessage(error instanceof Error ? error.message : "An error occurred during account activation");
      }
    };

    confirmAccount();
  }, [searchParams]);

  return (
    <div className="flex items-center justify-center min-h-screen bg-background p-4">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle className="text-center">Account Activation</CardTitle>
          <CardDescription className="text-center">
            {status === "loading" && "Activating your account..."}
            {status === "success" && "Your account has been activated!"}
            {status === "error" && "There was a problem activating your account"}
          </CardDescription>
        </CardHeader>
        <CardContent className="flex flex-col items-center gap-4">
          {status === "loading" && (
            <div className="flex flex-col items-center gap-2">
              <Loader2 className="h-16 w-16 text-primary animate-spin" />
              <p className="text-muted-foreground">Please wait while we activate your account</p>
            </div>
          )}

          {status === "success" && (
            <div className="flex flex-col items-center gap-4">
              <CheckCircle className="h-16 w-16 text-green-500" />
              <p>Your account has been successfully activated.</p>
              <Button asChild className="mt-2">
                <a href="/login">Go to Login</a>
              </Button>
            </div>
          )}

          {status === "error" && (
            <div className="flex flex-col items-center gap-4">
              <XCircle className="h-16 w-16 text-red-500" />
              <p className="text-center text-red-500">{errorMessage}</p>
              <Button asChild className="mt-2">
                <a href="/login">Go to Login</a>
              </Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}