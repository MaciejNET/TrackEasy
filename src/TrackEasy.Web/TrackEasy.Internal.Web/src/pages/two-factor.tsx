import {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import {useAuthStore} from "@/stores/auth-store.ts";
import {Button} from "@/components/ui/button.tsx";
import {Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle} from "@/components/ui/card.tsx";
import {Label} from "@/components/ui/label.tsx";
import {Loader2} from "lucide-react";
import {InputOTP, InputOTPGroup, InputOTPSlot} from "@/components/ui/input-otp.tsx";

export default function TwoFactor() {
  const navigate = useNavigate();
  const {submitTwoFactorCode, isLoading, error, needsTwoFactor, user} = useAuthStore();
  const [code, setCode] = useState("");

  useEffect(() => {
    if (!needsTwoFactor) {
      navigate("/login");
    }
  }, [needsTwoFactor, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const success = await submitTwoFactorCode({code});
    if (success) {
      navigate("/");
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-background">
      <Card className="w-[350px]">
        <CardHeader>
          <CardTitle>Two-Factor Authentication</CardTitle>
          <CardDescription>
            Enter the 6-digit code sent to your email {user?.email}
          </CardDescription>
        </CardHeader>
        <form onSubmit={handleSubmit}>
          <CardContent>
            <div className="grid w-full items-center gap-4">
              <div className="flex flex-col space-y-1.5">
                <Label htmlFor="code">Verification Code</Label>
                <InputOTP
                  id="code"
                  maxLength={6}
                  value={code}
                  onChange={setCode}
                  inputMode="numeric"
                >
                  <InputOTPGroup>
                    <InputOTPSlot index={0}/>
                    <InputOTPSlot index={1}/>
                    <InputOTPSlot index={2}/>
                    <InputOTPSlot index={3}/>
                    <InputOTPSlot index={4}/>
                    <InputOTPSlot index={5}/>
                  </InputOTPGroup>
                </InputOTP>
              </div>
              {error && (
                <div className="text-sm text-red-500">{error}</div>
              )}
            </div>
          </CardContent>
          <CardFooter className="flex justify-between">
            <Button type="submit" disabled={isLoading} className="w-full">
              {isLoading ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin"/>
                  Verifying...
                </>
              ) : (
                "Verify"
              )}
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
}
